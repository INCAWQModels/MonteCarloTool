using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MC
{
    class MC
    {
        public static void run(string[] args)
        {
            //run the model with a user-defined parameter set
            MCRunTimeParameters commandLineArguments = new MCRunTimeParameters();

            int MaxUnsuccessfulProposals = commandLineArguments.maximumUnsuccessfulJumps;
            parameterSet x = new parameterSet(commandLineArguments.modelParFile,
                commandLineArguments.minimumParFile, commandLineArguments.maximumParFile);

            //user specified (N) or random starting point (Y)
            if (commandLineArguments.randomStartPoint.Equals("Y", StringComparison.Ordinal))
            {
                x.randomizeValues();
                x.write(commandLineArguments.modelParFile);
            }

            x.write("bestParSet.par");
            parameterSet startingParameterSet = new parameterSet(commandLineArguments.modelParFile,
               commandLineArguments.minimumParFile, commandLineArguments.maximumParFile);

            InteractWithModel.runModel(commandLineArguments.commandString);

            // get a starting goodness of fit
            double referencePerformance = InteractWithModel.evaluatePerformanceStatistics(commandLineArguments.modelResultsFile,
                commandLineArguments.testStatisticColumn());
            double bestModelPerformance = referencePerformance;

            parameterSet bestParameterSet = new parameterSet("bestParSet.par",
                               commandLineArguments.minimumParFile, commandLineArguments.maximumParFile);

            parameterSet proposalParameterSet = new parameterSet(commandLineArguments.modelParFile,
                               commandLineArguments.minimumParFile, commandLineArguments.maximumParFile);

            StreamWriter logFile = new StreamWriter("log.csv");
            logFile.Write("Iteration,Ratio,TestStatistic\n");


            int unsuccessfulProposals = 0;
            //the following is needed to ensure proper initialization of the loop
            //it will force lastmodelPerformance to be less than candidatemodelperformance
            double candidateModelPerformance = referencePerformance - 1.0;
            double lastModelPerformance = referencePerformance;
            double performanceRatio = 1.0;
            Random MCCheck = new Random();

            for (long i = 0; i < commandLineArguments.iterations; i++)
            {
                proposalParameterSet.write(commandLineArguments.modelParFile);
                InteractWithModel.runModel(commandLineArguments.commandString);
                proposalParameterSet.writeToArray(commandLineArguments.arrayOutputFile, i.ToString());
                lastModelPerformance = candidateModelPerformance;
                candidateModelPerformance = InteractWithModel.evaluatePerformanceStatistics(commandLineArguments.modelResultsFile,
                    commandLineArguments.testStatisticColumn());
                Console.WriteLine("Model performance: {0} {1} {2:F4} {3:F4}", i, unsuccessfulProposals, referencePerformance, candidateModelPerformance);
                performanceRatio = candidateModelPerformance / lastModelPerformance;
                logFile.Write("{0}, {1:F4}\n", i, candidateModelPerformance);

                //sampler may have got lost, make a new random draw from the parameter space
                if (unsuccessfulProposals > MaxUnsuccessfulProposals)
                {
                    unsuccessfulProposals = -1;
                    //proposalParameterSet.updateValues("BASE",bestParameterSet);
                    //x.updateValues("BASE",bestParameterSet);
                    proposalParameterSet.randomizeValues();
                    x.randomizeValues();
                    //candidateModelPerformance = bestModelPerformance;
                    //lastModelPerformance = bestModelPerformance;
                    Console.WriteLine("Should be resetting {0} {1}", candidateModelPerformance, lastModelPerformance);
                }

                // jump, if we are moving in the right direction, keep making the same jump
                //otherwise make a random jump
                //
                //The right direction is assumed to occur when the ratio between candidateModelPerformance and 
                //lastModelPerfornace is greater than a {0,1} random number. This means that the sampler will
                //sometimes accept sub-optimal jumps
                //
                //Accept any imporvement in model performance and only try for acceptance when model does not
                //improve
                double testRatio = 1.0 - (lastModelPerformance - candidateModelPerformance) / Math.Abs(lastModelPerformance);
                double testStat = MCCheck.NextDouble();

                if (candidateModelPerformance > lastModelPerformance)
                {
                    x.updateValues("BASE", proposalParameterSet);
                    proposalParameterSet.jumpAgain();
                    unsuccessfulProposals = 0;
                }
                else
                {
                    if (testRatio > testStat)
                    {
                        proposalParameterSet.updateValues("BASE", x);
                        proposalParameterSet.fillValuesByJUMP(++unsuccessfulProposals);
                    }
                    else
                    {
                        Console.WriteLine("Accepting a less than optimal parameter set");
                        x.updateValues("BASE", proposalParameterSet);
                        proposalParameterSet.jumpAgain();
                        unsuccessfulProposals = 0;
                    }
                }


                //prevent huge jumps by resetting the jump size
                if ((candidateModelPerformance / bestModelPerformance) < 0.5)
                {
                    //if this happens, it means the sampler has got quite lost, resetting to the starting point 
                    //might be a good idea, alternately, setting the propsal parameter set to the previous best
                    //parameter set might be a good idea
                    Console.WriteLine("Poor performance {0} {1}...", candidateModelPerformance, bestModelPerformance);
                    proposalParameterSet.updateValues("BASE", startingParameterSet);
                    unsuccessfulProposals = 0;
                }

                //check to see if this is the best parameter yet, if so, save to disk
                if (candidateModelPerformance > bestModelPerformance)
                {
                    bestModelPerformance = candidateModelPerformance;
                    bestParameterSet.updateValues("BASE", proposalParameterSet);
                    x.write("bestParSet.par");
                }
            }
            logFile.Close();
            Console.WriteLine("Press [Enter] to exit, best parameter set is stored in bestParSet.par ...");
            Console.ReadLine();
        }
    }
}
