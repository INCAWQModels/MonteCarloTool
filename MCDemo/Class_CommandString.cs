using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace MC
{
    public class commandParameter
    {
        public string name;
        public string code;
        public string value;

        public commandParameter(string n, string c)
        {
            name = n;
            code = c;
        }

        public commandParameter(string n)
        {
            name = n;
        }

        public string appendToCommandLine()
        {
            string appendString = "";

            //only return something when the command argument has a value
            if (value.Length > 0) { appendString = " " + code + " " + value; }

            return appendString;
        }
    }

    public class commandParameterObservedFile : commandParameter
    {
        commandParameterObservedFile(string n, string c): base(n,c)
        {
            MCParameters.observedFileName = n;
        }
    }

    public class commandString
    {
        protected string modelExecutable;
        protected ArrayList commandArguments;
        public string commandLine;

        private void doPERSiSTStuff()
        {
            //this needs to be linked to the output in Summarize Results - right now they are not connected
            commandArguments.Add(new commandParameter("INCA output file", "-INCA"));
        }

        private void doINCA_CStuff()
        {
                    commandArguments.Add(new commandParameter("Organic Fertiliser Time Series File", "-of"));
                    commandArguments.Add(new commandParameter("Terrestrial PDC inputs", "-tdpc"));
                    commandArguments.Add(new commandParameter("Aquatic PDC inputs", "-adpc"));
                    commandArguments.Add(new commandParameter("Land Use Periods", "-lup"));
                    commandArguments.Add(new commandParameter("Deposition Time Series File", "-dep"));
                    commandArguments.Add(new commandParameter("Spatial Time Series", "-spatial"));
        }

        private void doINCAPathStuff()
        {
            //may not need any additional command line arguments
        }

        private void doINCANStuff()
        {
            //may not need any additional command line arguments
        }

        private void doINCAHgStuff()
        {
            //may not need any additional command line arguments
        }

        private void doINCAONTHEStuff()
        {
            //may not need any additional command line arguments
        }
        private void doINCAPEcoStuff()
        {
            doINCAPStuff();
        }

        private void doINCAPStuff()
        {
            commandArguments.Add(new commandParameter("Precip Type", "-precip"));           // Select which precipitation to use for infiltration calculation, defaults to "AP"
            //commandArguments.Add(new commandParameter("Snow AP (must enter -snowAP)"));   //a bit messy but gets snowAP in
            commandArguments.Add(new commandParameter("Snow correction", "-snowAP"));
            commandArguments.Add(new commandParameter("Grouping", "-grouping"));	        // Select how results files are grouped but branched systems, defaults to "all"
            commandArguments.Add(new commandParameter("Solid Fertiliser File", "-sfert"));	// Solid fertiliser time series filename        
            commandArguments.Add(new commandParameter("Liquid Fertiliser File", "-lfert"));	// Liquid fertiliser time series filename
            //commandArguments.Add(new commandParameter("Effluent", "-eff"));			    // Effluent time series filename
            //commandArguments.Add(new commandParameter("Abstraction", "-abs"));			// Abstraction time series filename
            commandArguments.Add(new commandParameter("Growth Periods File", "-grow"));		// Growth period time series filename
            commandArguments.Add(new commandParameter("Land Use Periods","-land"));			// Land use period time series filename
            //commandArguments.Add(new commandParameter("Structure File","-structure"));	// Structure file filename
            commandArguments.Add(new commandParameter("Multi-branch Spatial File","-spatial"));			// Spatially distributed time series filename (not needed for main stem-only applications)
            //commandArguments.Add(new commandParameter("Observed","-obs"));			    // Observed data filename. Writes goodness-of-fit coefficients to a file called 'coefficients.csv'
            commandArguments.Add(new commandParameter("Log file","-log"));			        // Write an error log to <filename>
            commandArguments.Add(new commandParameter("Reference Run Number","-ref"));		// Reference run number, written to error log
            commandArguments.Add(new commandParameter("Error File","-errfile"));	        // Select what to do with solver warnings:
        }

        private void doINCAToxStuff()
        {
            commandArguments.Add(new commandParameter("Organic Fertiliser Time Series","-ofert"));			// Organic fertiliser time series filename
            commandArguments.Add(new commandParameter("Terrestrial PDC Time Series","-opdc"));			// Organic (terrestrial) PDC time series filename
            commandArguments.Add(new commandParameter("Aquatic PDC Time Series","-apdc"));			// Aquatic PDC time series filename
            commandArguments.Add(new commandParameter("Contaminant Wet Deposition Time Series","-toxwetdep"));			// Contaminant wet deposition time series filename
            commandArguments.Add(new commandParameter("Contaminant Dry Deposition Time Series","-toxdrydep"));			// Contaminant dry deposition time series filename
            commandArguments.Add(new commandParameter("Contaminant Litterfall Time Series","-toxlitter"));			// Contaminant litter fall time series filename
            commandArguments.Add(new commandParameter("Contaminant Fertiliser Time Series","-toxfert"));			// Contaminant fertilizer time series filename
            commandArguments.Add(new commandParameter("Contaminant Atmospheric Concentration Time Series","-toxconc"));			// Contaminant atmospheric concentration time series filename
            commandArguments.Add(new commandParameter("Growth Periods","-grow"));			// Growth period time series filename
            commandArguments.Add(new commandParameter("Land Use Periods","-land"));			// Land use period time series filename
            commandArguments.Add(new commandParameter("Spatial Time Series","-spatial"));			// Spatially distributed time series filename (not needed for main stem-only applications)
            InteractWithModel.setNumberOfContaminants();
       }

        public commandString()
        {
            commandArguments = new ArrayList();

            commandArguments.Add(new commandParameter("Parameter File", "-par"));
            commandArguments.Add(new commandParameter("Data File", "-dat"));
            commandArguments.Add(new commandParameter("Observed Data File", "-obs"));
            commandArguments.Add(new commandParameter("Abstraction Time Series File", "-abs"));
            commandArguments.Add(new commandParameter("Effluent Time Series File", "-eff"));
            commandArguments.Add(new commandParameter("System structure file", "-structure"));


            switch (MCParameters.model)
            {
                case 1:
                case 8:
                case 10:
                    modelExecutable = "persist_cmd.exe -numbered ";
                    doPERSiSTStuff();
                    break;
                case 2:
                case 11:
                    modelExecutable = "inca_c_cmd.exe";
                    doINCA_CStuff();
                    break;
                case 3:
                    modelExecutable = "inca_pEco_cmd.exe -writeCoefficients";
                    doINCAPEcoStuff();
                    break;
                case 4:
                    modelExecutable = "inca_p_cmd.exe -writeCoefficients ";
                    doINCAPStuff();
                    break;
                case 5:
                    modelExecutable = "inca_contaminants_cmd.exe -writeCoefficients -snowAP -missing 0.0 ";
                    doINCAToxStuff();
                    break;
                case 6:
                    modelExecutable = "inca_path_cmd.exe -writeCoefficients ";
                    doINCAPathStuff();
                    break;
                case 7:
                    modelExecutable = "inca_hg_cmd.exe -writecoefficients ";
                    doINCAHgStuff();
                    break;
                case 9:
                    modelExecutable = "inca_on_the_cmd.exe -COUP -snow -writecoefficients ";
                    doINCAONTHEStuff();
                    break;
                case 12: 
                    modelExecutable = "inca_n_cmd.exe -writecoefficients ";
                    doINCANStuff();
                    break;
                default:
                   Console.WriteLine("This should not happen! Unrecognised model in Command string");
                   break;
            }
        }

        void write()
        {
            commandLine = modelExecutable;

            foreach (commandParameter c in commandArguments)
            {
                commandLine += c.appendToCommandLine();
            }
        }

        public void populate()
        {
            string tmpValue;

            foreach (commandParameter p in commandArguments)
            {
                //fix so that the appropriate parameter file name is always used in the simulations, 
                //regardless of what the user enters
                Console.Write("Please enter the name of the {0} : ", p.name);
                tmpValue = Console.ReadLine();
                //can assign tmpValue to p.value for all cases except parameter file name
                if (p.code.Equals("-par"))
                    p.value= setParFileName(tmpValue);
                else
                    p.value = tmpValue;
            }
            this.write();
        }

        private String setParFileName(string candidateParFileName)
        {
            //see if something other than the default parameter file name has been entered
            if(!(candidateParFileName.Equals(MCParameters.MCParFile,StringComparison.OrdinalIgnoreCase)))
            {
                try   { File.Copy(candidateParFileName, MCParameters.MCParFile, true); }
                catch { Console.WriteLine("Problems indentifying .par file"); }
            }
            return MCParameters.MCParFile;
        }
    }
}
