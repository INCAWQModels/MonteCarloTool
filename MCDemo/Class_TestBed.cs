using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MC
{
    class testBed
    {
        public static void test()
        {
            parameterSet m = new parameterSet("test.par", MCParameters.minParFile, MCParameters.maxParFile);
            m.write("out.par");

            parameterSet p = new parameterSet("test.par");
            if (File.Exists("test.csv"))
                File.Delete("test.csv");

            for (int i = 0; i < 10; i++)
                p.writeToArray(i.ToString(),"test.csv");
            parameterSet q = new parameterSet(MCParameters.minParFile);
            q.updateFromArray("5","test.csv");

            //InteractWithModel.runModel("persist_cmd.exe -par test.par -dat test.dat -obs test.obs -size small");
            //p.modelPerformance = InteractWithModel.evaluatePerformanceStatistics(Persist_Errors.csv", 2);
            //InteractWithModel.runModel("persist_cmd.exe -par out.par -dat test.dat -obs test.obs -size small");
            //m.modelPerformance = InteractWithModel.evaluatePerformanceStatistics("Persist_Errors.csv", 2);
            //Console.WriteLine("Model performance: {0} {1}", p.modelPerformance,m.modelPerformance);
            //q.write("q.par");
            //InteractWithModel.runModel("persist_cmd.exe -par q.par -dat test.dat -obs test.obs -size small");
            //q.modelPerformance = InteractWithModel.evaluatePerformanceStatistics("Persist_Errors.csv", 2);
            //Console.WriteLine("Model performance: {0} {1} {2}", p.modelPerformance, m.modelPerformance,q.modelPerformance);
            //Console.ReadLine();
        }

        public static void LHSTest()
        {
            parameterSet m = new parameterSet("test.par", MCParameters.minParFile, MCParameters.maxParFile);
            LHSController LHS = new LHSController(10,m);
            LHS.shuffle();

            if (File.Exists("lhs.csv"))
            {
                try { File.Delete("lhs.csv"); }
                catch { Console.WriteLine("Could not delete lhs.csv"); }
            }

            //only needed for debugging
            parameterSet min = new parameterSet(MCParameters.minParFile);
            parameterSet max = new parameterSet(MCParameters.maxParFile);
            min.writeToArray("min", "lhs.csv");
            max.writeToArray("max", "lhs.csv");

            //for (int i = 0; i < 10; i++)
            //{
            //    m.LHS_Sample(i,LHS);
            //    m.write("lhs.par");
            //    InteractWithModel.runModel("persist_cmd.exe -par lhs.par -dat test.dat -obs test.obs -size small");
            //    double ps1 = InteractWithModel.evaluatePerformanceStatistics("Persist_Errors.csv", 2);
            //    Console.WriteLine("{0} {1}", i, ps1);
            //    m.writeToArray(i.ToString(), "lhs.csv");
            //}
            m.write("lhs.par");
        }
    }
}
