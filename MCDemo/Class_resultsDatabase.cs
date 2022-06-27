﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.IO;

namespace MC
{
    class resultsDatabase
    {
        protected OdbcConnection localConnection;

        public resultsDatabase()
        {
            localConnection = new OdbcConnection();
        }

        public void cleanUp()
        {
            string path = Directory.GetCurrentDirectory();
            //Console.WriteLine(path);
            MCParameters.databaseFileName = path + "\\mc.accdb";

            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            //Console.WriteLine(localConnectionString);

            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (OdbcException ex) {Console.WriteLine(ex.Message); }
            //only try to clean up if the connection is open
            if (localConnection.State == ConnectionState.Open)
            {
                executeSQLCommand("DELETE * FROM ParNames");
                executeSQLCommand("DELETE * FROM ParList");
                executeSQLCommand("DELETE * FROM SortedParameters");
                executeSQLCommand("DELETE * FROM CoefficientWeights");
                executeSQLCommand("DELETE * FROM SortedParameters");
                executeSQLCommand("DROP TABLE Coefficients");
                executeSQLCommand("DROP TABLE Results");
                executeSQLCommand("DROP TABLE IncaInputs");
                executeSQLCommand("DROP TABLE Observations");
                executeSQLCommand("DROP TABLE ParameterSensitivitySummary");
                localConnection.Close();
            }
            else { Console.WriteLine("Could not clean up database"); };
        }

        public void processParameterData()
        {
            string path = Directory.GetCurrentDirectory();
            //Console.WriteLine(path);
            MCParameters.databaseFileName = path + "\\mc.accdb";

            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            //Console.WriteLine(localConnectionString);

            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }
            //only try to clean up if the connection is open
            if (localConnection.State == ConnectionState.Open)
            {
                executeSQLCommand("INSERT INTO SortedParameters ( ParID, ParameterValue, RunID )" +
                    "SELECT ParList.ParID, ParList.NumericValue, ParList.RunID " +
                    "FROM ParList INNER JOIN[102 Sampled Pars] ON ParList.ParID = [102 Sampled Pars].ParID " +
                    "ORDER BY ParList.ParID, ParList.NumericValue;"
                );
                localConnection.Close();
            }
            else { 
                Console.WriteLine("Could not fix parameters");
                Console.ReadLine();
            };
        }

        public void _processParameterData()
        {
            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }

            string SQLString = "INSERT INTO SortedParameters(ParID, ParameterValue, RunID ) " +
                "SELECT ParList.ParID, ParList.NumericValue, ParList.RunID " +
                "FROM ParList INNER JOIN[102 Sampled Pars] ON ParList.ParID = [102 Sampled Pars].ParID " +
                "ORDER BY ParList.ParID, ParList.NumericValue";
            
            OdbcCommand tmp = new OdbcCommand(SQLString, localConnection);
            try { tmp.ExecuteNonQuery(); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            tmp.Dispose();
        }

        public void createParameterSensitivitySummaryTable()
        {
            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }

            string SQLstring = "SELECT[116 Statistics Summary].ParName, [116 Statistics Summary].ParID, [116 Statistics Summary].D, " +
                " [116 Statistics Summary].MinOfNumericValue, [116 Statistics Summary].MaxOfNumericValue, [116 Statistics Summary].xRange, " +
                " [116 Statistics Summary].z, [116 Statistics Summary].p INTO ParameterSensitivitySummary FROM [116 Statistics Summary]";
            executeSQLCommand(SQLstring);
            localConnection.Close();
        }

        private void notYetImplemented()
        {
            //write a message to let the user know this feature does not yet exist for this verions of INCA/PERSiST
            Console.WriteLine("This feature is not yet implemented for this verion of INCA");
            Console.WriteLine("Text files are generated which can be used for subsequent analysis");
        }

        public void makeResultsTable()
        {
            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }
            switch (MCParameters.model)
            {
                case 1: //PERSiST 1.4
                case 8: // PERSiST 1.6
                case 10: //PERSiST v2
                    makePERSiSTResultsTable();
                    makeINCAInputsTable();
                    break;
                case 2: //INCA-C
                    makeINCA_CResultsTable();
                    break;
                case 3: //INCA-PEco
                case 4: //INCA-P
                case 5: //INCA-Contaminants
                case 6: //INCA-Path
                case 11: //INCA-C v2.x
                case 12: //INCA-N Classic
                    notYetImplemented();
                    break;
                case 7:
                    makeINCA_HgResultsTable();
                    break;
                case 9:
                    makeINCA_ONTHEResultsTable();
                    break;
                default:
                    Console.Write("Something has gone wrong when making the COEFFICIENTS table");
                    break;
            }
            localConnection.Close();
        }

        private void makePERSiSTResultsTable()
        {
            string SQLString;

            SQLString = "CREATE TABLE Results (" +
                "RUN        INTEGER," +
                "RowNumber  INTEGER," +
                "Reach      TEXT(255)," +
                "TerrestrialInput   DOUBLE," +
                "Flow       DOUBLE," +
                "DateStamp  DATE)";
            executeSQLCommand(SQLString);
        }

        private void makeINCA_CResultsTable()
        {
            string SQLString;

            SQLString = "CREATE TABLE Results (" +
                "RUN                INTEGER," +
                "RowNumber          INTEGER," +
                "Reach              TEXT(255)," +
                "Flow               DOUBLE," +
                "DateStamp          DATE)";
            executeSQLCommand(SQLString);
        }

        private void makeINCA_HgResultsTable()  // needs to be cleaned up for INCA_Hg outputs
        {
            string SQLString;

            SQLString = "CREATE TABLE Results (" +
                "RUN        INTEGER," +
                "RowNumber  INTEGER," +
                "Reach      TEXT(255)," +
                "Flow       DOUBLE," +
                "DateStamp  DATE)";
            executeSQLCommand(SQLString);
        }

        private void makeINCAInputsTable()
        {
            string SQLString;

            SQLString = "CREATE TABLE INCAInputs (" +
                "FileName       TEXT(255)," +
                "RUN            INTEGER," +
                "RowNumber      INTEGER,"+
                "SMD            DOUBLE," +
                "HER            DOUBLE," +
                "T              DOUBLE," +
                "P              DOUBLE," +
                "DateStamp      DATE)";
            executeSQLCommand(SQLString);
        }

        private void makeINCA_ONTHEResultsTable()
        {
            string SQLString;

            SQLString = "CREATE TABLE Results (" +
                "FileName       TEXT(255)," +
                "RUN            INTEGER," +
                "RowNumber      INTEGER," +
                "FLOW           DOUBLE," +
                "NITRATE        DOUBLE," +
                "AMMONIUM       DOUBLE," +
                "VOLUME         DOUBLE," +
                "DON            DOUBLE," +
                "VELOCITY       DOUBLE," +
                "WIDTH          DOUBLE," +
                "DEPTH          DOUBLE," +
                "AREA           DOUBLE," +
                "PERIMETER      DOUBLE," +
                "RADIUS         DOUBLE," +
                "RESIDENCETIME  DOUBLE," +
                "DateStamp      DATE)";
            executeSQLCommand(SQLString);
        }

        private void makeINCA_PResultsTable()
        {
            string SQLString;

            SQLString = "CREATE TABLE Results (" +
                "FileName           TEXT(255), " +
                "RUN                INTEGER," +
                "RowNumber          INTEGER," +
                "Discharge          DOUBLE, " +
                "Volume             DOUBLE, " +
                "Velocity           DOUBLE, " +
                "WaterDepth         DOUBLE, " +
                "StreamPower        DOUBLE, " +
                "ShearVelocity      DOUBLE, " +
                "MaxEntGrainSize    DOUBLE, " +
                "MoveableBedMass    DOUBLE, " +
                "EntrainmentRate    DOUBLE, " +
                "DepositionRate     DOUBLE, " +
                "BedSediment        DOUBLE, " +
                "SuspendedSediment  DOUBLE, " +
                "DiffuseSediment    DOUBLE, " +
                "WaterCOlumnTDP     DOUBLE, " +
                "WaterColumnPP      DOUBLE," +
                "WCSorptionRelease  DOUBLE, " +
                "StreamBedTDP       DOUBLE, " +
                "StreamBedPP        DOUBLE, " +
                "BedSorptionRelease DOUBLE, " +
                "MacrophyteMass     DOUBLE, " +
                "EpiphyteMass       DOUBLE, " +
                "WaterColumnTP      DOUBLE, " +
                "WaterColumnSRP     DOUBLE, " +
                "WaterTemperature   DOUBLE, " +
                "TDPInput           DOUBLE, " +
                "PPInput            DOUBLE, " +
                "WaterColumnEPC0    DOUBLE, " +
                "StreamBedEPC0      DOUBLE, " +
                "DateStamp          Date)";
            executeSQLCommand(SQLString);
        }

        public void makeObservationsTable()
        {
            //we have the same observations table for each version of the model so do not have to do anything special
            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }
            string SQLString = "CREATE TABLE Observations (Reach  TEXT(255)," +
                "Parameter  TEXT(255)," +
                "Value      DOUBLE," +
                "QC         TEXT(255)" +
                "DateStamp  DATE)";
            executeSQLCommand(SQLString);
            localConnection.Close();
        }

        public void makeCoefficientsTable()
        {
            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }
            switch (MCParameters.model)
            {
                case 1: //PERSiST 1.4
                case 8: // PERSiST 1.6
                    makePERSiSTCoefficientsTable();
                    break;
                case 2: //INCA-C
                case 11: //INCA-C 2.x
                    makeINCA_CCoefficientsTable();
                    break;
                case 3: //INCA-PEco
                    makeINCA_PEcoCoefficientsTable();
                    break;
                case 4: //INCA-P
                    makeINCA_PCoefficientsTable();
                    break;
                case 5: //INCA-Contaminants
                    notYetImplemented();
                    break;
                case 6: //INCA-Path
                    notYetImplemented();
                    break;
                case 7:
                    makeINCA_HgCoefficientsTable();
                    break;
                case 9:
                    makeINCA_ONTHECoefficientsTable();
                    break;
                case 10: //PERSiST v2
                    makePERSiST_v2CoefficientsTable();
                    break;
                case 12: //INCA-N
                    makeINCA_NCoefficientsTable();
                    break;
                default:
                    Console.Write("Something has gone wrong when making the COEFFICIENTS table");
                    Console.ReadLine();
                    break;
            }
            localConnection.Close();
        }

        private void makeDefaultCoefficientsTable()
        {
            string SQLString;

            SQLString = "CREATE TABLE Coefficients (" +
                "RUN        INTEGER," +
                "RowNumber  INTEGER," +
                "Reach      TEXT(255)," +
                "Parameter  TEXT(255)," +
                "R2         DOUBLE," +
                "NS         DOUBLE," +
                "RMSE       DOUBLE," +
                "RE         DOUBLE," +
                "DateStamp  DATE)";
            executeSQLCommand(SQLString);
        }

        private void makeINCA_NCoefficientsTable()
        {
            makeDefaultCoefficientsTable();
        }

        private void makeINCA_ONTHECoefficientsTable()
        {
            string SQLString;

            SQLString = "CREATE TABLE Coefficients (" +
                "RUN        INTEGER," +
                "RowNumber  INTEGER," +
                "Reach      TEXT(255)," +
                "Parameter  TEXT(255)," +
                "R2         DOUBLE," +
                "NS         DOUBLE," +
                "logNS      DOUBLE," +
                "AD         DOUBLE," +
                "VAR        DOUBLE," +
                "KGE        DOUBLE," +
                "DateStamp  DATE)";
            executeSQLCommand(SQLString);
        }

        private void makeINCA_PEcoCoefficientsTable()
        {
            string SQLString;

            SQLString = "CREATE TABLE Coefficients (" +
                "RUN        INTEGER," +
                "RowNumber  INTEGER," +
                "Reach      TEXT(255)," +
                "Parameter  TEXT(255)," +
                "R2         DOUBLE," +
                "NS         DOUBLE," +
                "logNS      DOUBLE, " +
                "RMSE       DOUBLE," +
                "AD         DOUBLE," +
                "VR         DOUBLE, "+
                "KGE        DOUBLE, " +
                "CAT_B      DOUBLE,"+
                "CAT_C      DOUBLE,"+   
                "CAT_CA     DOUBLE,"+
                "CAT_CB     DOUBLE,"+
                "DateStamp  DATE)";
            executeSQLCommand(SQLString);
        }
        
        private void makeINCA_PCoefficientsTable()
        {
            string SQLString;

            SQLString = "CREATE TABLE Coefficients (" +
                "RUN        INTEGER," +
                "RowNumber  INTEGER," +
                "Reach      TEXT(255)," +
                "Parameter  TEXT(255)," +
                "R2         DOUBLE," +
                "NS         DOUBLE," +
                "RMSE       DOUBLE," +
                "RE         DOUBLE," +
                "VR         DOUBLE," +
                "DateStamp  DATE)";
            executeSQLCommand(SQLString);
        }

        private void makePERSiSTCoefficientsTable()
        {
            string SQLString;

            SQLString = "CREATE TABLE Coefficients (" +
                "RUN        INTEGER," +
                "RowNumber  INTEGER," +
                "Reach      TEXT(255)," +
                "R2         DOUBLE," +
                "NS         DOUBLE," +
                "LOG_NS     DOUBLE," +
                "RMSE       DOUBLE," +
                "RE         DOUBLE," +
                "AD         DOUBLE," +
                "VAR        DOUBLE," +
                "N          DOUBLE," +
                "N_RE       DOUBLE," +
                "SS         DOUBLE," +
                "LOG_SS     DOUBLE," +
                "DateStamp  DATE)";
            executeSQLCommand(SQLString);
        }

        private void makePERSiST_v2CoefficientsTable()
        {
            string SQLString;

            SQLString = "CREATE TABLE Coefficients (" +
                "RUN        INTEGER," +
                "RowNumber  INTEGER," +
                "Reach      TEXT(255)," +
                "Parameter  TEXT(255)," +
                "R2         DOUBLE," +
                "NS         DOUBLE," +
                "LOG_NS     DOUBLE," +
                "RMSE       DOUBLE," +
                "RE         DOUBLE," +
                "AD         DOUBLE," +
                "VAR        DOUBLE," +
                "N          DOUBLE," +
                "N_RE       DOUBLE," +
                "SS         DOUBLE," +
                "LOG_SS     DOUBLE," +
                "DateStamp  DATE)";
            executeSQLCommand(SQLString);
        }
        private void makeINCA_CCoefficientsTable()
        {
            makeDefaultCoefficientsTable();
        }

        private void makeINCA_HgCoefficientsTable()
        {
            makeDefaultCoefficientsTable();
        }

        //take the summary file of results and write them all to the database (note - may want to change this later
        //so as to write one set of results at a time
        public void writeResults()
        {
            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }
            switch (MCParameters.model)
            {
                case 1: //PERSiST 1.4
                case 8: //PERSiST 1.6
                    //writeINCAResultsFromPERSiST();
                    //writePERSiSTResults();
                    break;
                case 2: //INCA-C
                    notYetImplemented();
                    break;
                case 3: //INCA-PEco
                    notYetImplemented();
                    break;
                case 4: //INCA-P
                    notYetImplemented();
                    break;
                case 5: //INCA-Contaminants
                    notYetImplemented();
                    break;
                case 6: //INCA-Path
                    notYetImplemented();
                    break;
                case 7: //INCA-Hg
                    notYetImplemented();
                    break;
                case 9: //INCA_ONTHE
                    notYetImplemented();
                    break;
                default:
                    Console.WriteLine("Something has gone wrong when populating the RESULTS table");
                    break;
            }
            localConnection.Close();
        }

        /*
        private string ReadFromFile(string FilePath)
        {
            string readText = File.ReadAllText(FilePath);
            return readText;
        }

        private void AddToDatabase()
        {
        string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
          
            OleDbConnection connection = new OleDbConnection(localConnectionString);
            string cmdstring = "insert into TABLENAME (BarCode) Values (?)";
            OleDbCommand command = new OleDbCommand(cmdstring, connection);
            command.Parameters.AddWithValue("?",ReadFromFile("Text File Path"));
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

        }
        */

        //this is really slow, needs to be refactored before it can be useful
        private void writePERSiSTResultsToDatabase()
        {
            for (var i = 0; i < MCParameters.splitsToUse; i++)
            {
                string fileName = MCParameters.resultFileNameStub + i.ToString() + ".txt";
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    string reach="";
                    while ((line = sr.ReadLine()) != null)
                    {
                        string SQLString="INSERT INTO Results (RUN, RowNumber, Reach, TerrestrialInput, Flow, DateStamp) VALUES (";

                        string[] fields = line.Split(MCParameters.separatorChar);
                        //check if the current row is a header row or a data row (based on the number of fields)
                        if(fields.Length==3)
                        {
                            reach = fields[2];
                            //give some evidence of activity
                            Console.WriteLine("Processing results for iteration " + fields[0] + ", reach " + reach);
                        }
                        else
                        {
                            SQLString = SQLString +
                                fields[0] + ", " +        //RUN
                                fields[1] + ", '" +        //RowNumber
                                reach + "', " +          //Reach
                                fields[2] + "," +         //Diffuse Inputs from land phase
                                fields[3] + ", DATE())";
                        }

                        using (StreamWriter SQLCheck = new StreamWriter("SQLCheck.txt"))
                        {
                            SQLCheck.WriteLine(SQLString);
                        }
                        
                        OdbcCommand tmp = new OdbcCommand(SQLString, localConnection);
                        try { tmp.ExecuteNonQuery(); }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        tmp.Dispose();
                    }
                }
            }
        }

        private void writePERSiSTResults()
        {
            string PERSiSTOutputFile = "PERSiSTResults.txt";
            File.Create(PERSiSTOutputFile).Dispose();

            for (var i = 0; i < MCParameters.splitsToUse; i++)
            {
                string fileName = MCParameters.resultFileNameStub + i.ToString() + ".txt";
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    string reach = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] fields = line.Split(MCParameters.separatorChar);
                        //check if the current row is a header row or a data row (based on the number of fields)
                        if (fields.Length == 3)
                        {
                            reach = fields[2];
                            //give some evidence of activity
                            Console.WriteLine("Processing results for iteration " + fields[0] + ", reach " + reach);
                        }
                        else
                        {
                            string resultString =
                                fields[0] + ", " +       //RUN
                                fields[1] + ", '" +      //RowNumber
                                reach + "', " +          //Reach
                                fields[2] + ", " +       //Diffuse Inputs from land phase
                                fields[3];               //flow
                            //check that we actually have numeric results, and if we do, write them
                            double tst;
                            try {
                                tst = Convert.ToDouble(fields[2]);
                                using (FileStream fs = new FileStream(PERSiSTOutputFile, FileMode.Append, FileAccess.Write))
                                using (StreamWriter sw = new StreamWriter(fs))
                                { sw.WriteLine(resultString); }
                                }
                            catch { }
                        }
                    }
                }
            }
        }

        //this is really slow, needs to be refactored before it can be useful
        private void writeINCAResultsFromPERSiSTToDatabase()
        {
            //assume that each candidate INCA inputs file has been generated in the current model run
            //this should work as all candidate input files are cleaned up earlier
            string[] resultFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), MCParameters.INCAFileNameStub + "*.txt");
            foreach (string f in resultFiles)
            {
                using (StreamReader sr = new StreamReader(f))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] fields = line.Split('\t');

                        using (StreamWriter SQLCheck = new StreamWriter("SQLCheck.txt"))
                        {
                            SQLCheck.WriteLine("Fields {0}",fields.Length);
                        }

                        try
                            {
                            string SQLString = "INSERT INTO INCAInputs (FileName, RUN, RowNumber, SMD, HER, T, P, DateStamp)" +
                                "VALUES (' " + f + "', " +
                                fields[0] + "," +        //Run
                                fields[1] + "," +       //RowNumber
                                fields[2] + "," +       //SMD
                                fields[3] + "," +       //HER
                                fields[4] + "," +       //T
                                fields[5] + "," +       //P
                                "DATE())";

                            using (StreamWriter SQLCheck = new StreamWriter("SQLCheck.txt"))
                            {
                                SQLCheck.WriteLine(SQLString);
                            }

                            executeSQLCommand(SQLString);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }

        private void writeINCAResultsFromPERSiST()
        {
            string INCASummaryFile = "INCASummary.txt";
            File.Create(INCASummaryFile).Dispose();

            //assume that each candidate INCA inputs file has been generated in the current model run
            //this should work as all candidate input files are cleaned up earlier
            string[] resultFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), MCParameters.INCAFileNameStub + "*.txt");
            foreach (string f in resultFiles)
            {
                using (StreamReader sr = new StreamReader(f))
                {
                    string line;
                    string resultString;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] fields = line.Split('\t');

                        try
                        {
                            resultString = f + ", " +
                                fields[0] + "," +        //Run
                                fields[1] + "," +       //RowNumber
                                fields[2] + "," +       //SMD
                                fields[3] + "," +       //HER
                                fields[4] + "," +       //T
                                fields[5];             //P

                            using (FileStream fs = new FileStream(INCASummaryFile, FileMode.Append, FileAccess.Write))
                            using (StreamWriter sw = new StreamWriter(fs))
                            { sw.WriteLine(resultString); }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }

        //take the summary file of coefficients and write them all to the database (note - may want to change this later
        //so as to write one set of coefficients at a time
        public void writeCoefficients()
        {
            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }
            switch (MCParameters.model)
            {
                case 1: //PERSiST 1.4
                case 8: // PERSiST 1.6
                    writePERSiSTCoefficients();
                    break;
                case 2:     //INCA-C v.1.x
                case 7:     // INCA-Hg
                    writeGenericINCA_Coefficients();
                    break;
                case 3: //INCA-PEco
                    writeINCA_PEcoCoefficients();
                    break;
                case 4: //INCA-P
                    notYetImplemented();
                    break;
                case 5: //INCA-Contaminants
                    notYetImplemented();
                    break;
                case 6: //INCA-Path
                    notYetImplemented();
                    break;
                case 9: //INCA_ONTHE
                    writeINCA_ONTHECoefficients();
                    break;
                case 10: //PERSIST v2
                    writePERSiST_v2Coefficients();
                    break;
                case 11: //INCA_C v.2
                    notYetImplemented();
                    break;
                case 12:    // INCA-N v.1.x
                    writeINCA_NCoefficients();
                    break;
                default:
                    Console.WriteLine("Something has gone wrong when populating the COEFFICIENTS table");
                    break;
            }
            localConnection.Close();
        }

        private void writePERSiSTCoefficients()
        {
            using (StreamReader sr = new StreamReader(MCParameters.coefficientsSummaryFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    {
                        try
                        {
                            string[] fields = line.Split(MCParameters.separatorChar);
                            if ((fields[1]).Equals("0")) { Console.WriteLine(line); }
                            else
                            {
                                string SQLString = "INSERT INTO Coefficients " +
                                    "(RUN, RowNumber, Reach, R2, NS, LOG_NS, RMSE, RE, AD, VAR, N, N_RE, SS, LOG_SS, DateStamp) VALUES (" +
                                    fields[0] + ", " +
                                    fields[1] + ", '" +
                                    fields[2] + "', " +
                                    fields[3] + ", " +
                                    fields[4] + "," +
                                    fields[5] + ", " +
                                    fields[6] + ", " +
                                    fields[7] + ", " +
                                    fields[8] + ", " +
                                    fields[9] + ", " +
                                    fields[10] + ", " +
                                    fields[11] + ", " +
                                    fields[12] + ", " +
                                    fields[13] + ", " +
                                    " DATE())";
                                //Console.WriteLine(SQLString);
                                using (StreamWriter SQLCheck = new StreamWriter("SQLCheck.txt"))
                                {
                                    SQLCheck.WriteLine(SQLString);
                                }
                                OdbcCommand tmp = new OdbcCommand(SQLString, localConnection);
                                try { tmp.ExecuteNonQuery(); }
                                catch (Exception ex) { Console.WriteLine(ex.Message); }
                                tmp.Dispose();
                            }
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                }
            }
        }

        private void writePERSiST_v2Coefficients()
        {
            using (StreamReader sr = new StreamReader(MCParameters.coefficientsSummaryFile))
            {
                string line;
                string reachName = "undefined";
                while ((line = sr.ReadLine()) != null)
                {
                    {
                        try
                        {
                            string[] fields = line.Split(MCParameters.separatorChar);
                            //
                            // get the reach name from the header row so ensure that therE are enough columns and that fields[3] is non-numeric
                            // continue to populate coefficeints if thre are enough fields
                            //
                            if (fields.Length > 3)
                            {
                                int rownum = int.Parse(fields[1]);
                                if ((rownum % 7) == 0)
                                {
                                    reachName = fields[2];
                                }
                                else
                                {
                                    string SQLString = "INSERT INTO Coefficients " +
                                        "(RUN, RowNumber, Reach, Parameter, R2, NS, LOG_NS, RMSE, RE, AD, VAR, N, N_RE, SS, LOG_SS, DateStamp) VALUES (" +
                                        fields[0] + ", " +
                                        fields[1] + ", '" +
                                        reachName + "', '" +
                                        fields[2] + "', " +
                                        fields[3] + ", " +
                                        fields[4] + "," +
                                        fields[5] + ", " +
                                        fields[6] + ", " +
                                        fields[7] + ", " +
                                        fields[8] + ", " +
                                        fields[9] + ", " +
                                        fields[10] + ", " +
                                        fields[11] + ", " +
                                        fields[12] + ", " +
                                        fields[13] + ", " +
                                        " DATE())";
                                    //Console.WriteLine(SQLString);
                                    using (StreamWriter SQLCheck = new StreamWriter("SQLCheck.txt"))
                                    {
                                        SQLCheck.WriteLine(SQLString);
                                    }
                                    OdbcCommand tmp = new OdbcCommand(SQLString, localConnection);
                                    try { tmp.ExecuteNonQuery(); }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                                    tmp.Dispose();
                                }
                            }
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                }
            }
        }
        private void writeGenericINCA_Coefficients()
        {
            using (StreamReader sr = new StreamReader(MCParameters.coefficientsSummaryFile))
            {
                string line;
                string reachName = "undefined";
                while ((line = sr.ReadLine()) != null)
                {
                    {
                        try
                        {
                            string[] fields = line.Split(MCParameters.separatorChar);
                            //
                            // get the reach name from the header row so ensure that there are enough columns and that fields[3] is non-numeric
                            // continue to populate coefficients if thre are enough fields
                            //
                            if (fields.Length > 2)
                            {
                                int rownum = int.Parse(fields[1]);
                                if ((rownum % 8) == 0)
                                {
                                    reachName = fields[2];
                                }
                                else
                                {
                                    string SQLString = "INSERT INTO Coefficients " +
                                        "(RUN, RowNumber, Reach, Parameter, R2, NS, RMSE, RE, DateStamp) VALUES (" +
                                        fields[0] + ", " +
                                        fields[1] + ", '" +
                                        reachName + "', '" +
                                        fields[2] + "', " +
                                        fields[3] + ", " +
                                        fields[4] + "," +
                                        fields[5] + ", " +
                                        fields[6] + ", " +
                                        " DATE())";
                                    //Console.WriteLine(SQLString);
                                    using (StreamWriter SQLCheck = new StreamWriter("SQLCheck.txt"))
                                    {
                                        SQLCheck.WriteLine(SQLString);
                                    }
                                    OdbcCommand tmp = new OdbcCommand(SQLString, localConnection);
                                    try { tmp.ExecuteNonQuery(); }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                                    tmp.Dispose();
                                }
                            }
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                }
            }
        }

        private void writeINCA_NCoefficients()
        {
            using (StreamReader sr = new StreamReader(MCParameters.coefficientsSummaryFile))
            {
                string line;
                string reachName = "undefined";
                while ((line = sr.ReadLine()) != null)
                {
                    {
                        try
                        {
                            string[] fields = line.Split(MCParameters.separatorChar);
                            //
                            // get the reach name from the header row so ensure that there are enough columns and that fields[3] is non-numeric
                            // continue to populate coefficients if thre are enough fields
                            //
          
                            if (fields.Length > 2)
                            {
                                int rownum = int.Parse(fields[1]);
                                if ((rownum % 6) == 0)
                                {
                                    reachName = fields[2];
                                }
                                else
                                {
                                    string SQLString = "INSERT INTO Coefficients " +
                                        "(RUN, RowNumber, Reach, Parameter, R2, NS, RMSE, RE, DateStamp) VALUES (" +
                                        fields[0] + ", " +
                                        fields[1] + ", '" +
                                        reachName + "_Reach', '" +
                                        fields[2] + "', " +
                                        fields[3] + ", " +
                                        fields[4] + "," +
                                        fields[5] + ", " +
                                        fields[6] + ", " +
                                        " DATE())";
                                    //Console.WriteLine(SQLString);
                                    using (StreamWriter SQLCheck = new StreamWriter("SQLCheck.txt"))
                                    {
                                        SQLCheck.WriteLine(SQLString);
                                    }
                                    OdbcCommand tmp = new OdbcCommand(SQLString, localConnection);
                                    try { tmp.ExecuteNonQuery(); }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                                    tmp.Dispose();
                                }
                            }
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                }
            }
        }
        private void writeINCA_ONTHECoefficients()
        {
            try
            {
                using (StreamReader sr = new StreamReader(MCParameters.coefficientsSummaryFile))
                {
                    string line;
                    string reachName = "";

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] fields = line.Split(MCParameters.separatorChar);
                        if (fields.Length == 3) //we have a reach name
                            {
                                Console.WriteLine(line);
                                reachName = fields[2];
                            }
                        else
                        {
                            if (fields.Length > 4)   //skip short lines, i.e., header and footer
                            {
                                string SQLString;

                                SQLString = "INSERT INTO Coefficients (Run, RowNumber, Reach, Parameter, R2, NS, logNS, AD, VAR, KGE, DateStamp) VALUES (" +
                                    fields[0] + ", " +
                                    fields[1] + ",'" +
                                    reachName + "', '" +
                                    fields[2] + "', " +         //parameter name
                                    fields[3] + "," +           //R2
                                    fields[4] + ", " +          //Nash Sutcliffe
                                    fields[5] + ", " +          //logNS
                                    fields[7] + ", " +          //AD
                                    fields[8] + ", " +          //Var
                                    fields[13] + ", " +           //KGE
                                    " Date())";    //RE and date


                                using (StreamWriter SQLCheck = new StreamWriter("SQLCheck.txt"))
                                {
                                    SQLCheck.WriteLine(SQLString);
                                }
                                OdbcCommand tmp = new OdbcCommand(SQLString, localConnection);
                                try { tmp.ExecuteNonQuery(); }
                                catch (Exception ex) {
                                    Console.WriteLine(SQLString);
                                    Console.WriteLine(ex.Message);
                                    //Console.ReadLine();
                                }
                                tmp.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /* should not be necessary
        private void writeINCA_HgCoefficients()
        {
            writeDefaultCoefficients();
        }
        */

        private void writeDefaultCoefficients()
        { 
            //open a connection
            //string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            //localConnection.ConnectionString = localConnectionString;
            //try { localConnection.Open(); }
            //catch (Exception ex) { Console.WriteLine(ex.Message); }
            
            try
            {
                using (StreamReader sr = new StreamReader(MCParameters.coefficientsSummaryFile))
                {
                    string line;
                    for (int k = 0; k < MCParameters.runsToOrganize; k++)
                    {
                        for (int i = 0; i < MCParameters.numberOfReaches; i++)
                        {
                            string reachName = "";
                            for (int j = 0; j <= 6; j++)
                            {
                                line = sr.ReadLine();
                                string[] fields = line.Split(MCParameters.separatorChar);
                                //Console.WriteLine(j.ToString() + ": " + line);
                                switch (j)
                                {
                                    case 0:
                                        {
                                            reachName = fields[2];
                                            break;
                                        }
                                    case 2:
                                    //case 3:
                                    case 4:
                                    case 5:
                                        {
                                            string SQLString;
                                            SQLString = "INSERT INTO Coefficients (Run, RowNumber, Reach, Parameter, R2, NS, RMSE, RE, DateStamp) VALUES (" +
                                                fields[0] + ", " +
                                                j.ToString() + ",'" +
                                                reachName + "', '" +
                                                fields[2] + "', " +      //parameter name
                                                fields[3] + "," +        //R2
                                                fields[4] + ", " +      //Nash Sutcliffe
                                                fields[5] + ", " +       //RMSE
                                                fields[6] + ", Date())";  //RE and date
                                            Console.WriteLine(SQLString);
                                            using (StreamWriter SQLCheck = new StreamWriter("SQLCheck.txt"))
                                            {
                                                SQLCheck.WriteLine(SQLString);
                                            }
                                            OdbcCommand tmp = new OdbcCommand(SQLString, localConnection);
                                            try { tmp.ExecuteNonQuery(); }
                                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                                            tmp.Dispose();
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void writeINCA_PEcoCoefficients()
        {
            try
            {
                using (StreamReader sr = new StreamReader(MCParameters.coefficientsSummaryFile))
                {
                    string line;
                    string reachName = "";
                    
                    while ((line = sr.ReadLine()) != null)
                        {
                        string[] fields = line.Split(MCParameters.separatorChar);
                        // get reach names, this will happen when row number is a multiple of 16
                        //right now exceptions are not caught
                        int fieldNumber = Int32.Parse(fields[1]);
                        if ((fieldNumber % 16) == 0)
                        {
                            Console.WriteLine(line);
                            reachName = fields[2];
                        }
                        if (fields.Length>10)   //skip short lines, i.e., header and footer
                        {
                            string SQLString;
                            
                            SQLString = "INSERT INTO Coefficients (Run, RowNumber, Reach, Parameter, R2, NS, logNS, RMSE, AD, VR, KGE, CAT_B, CAT_C, Cat_Ca, Cat_Cb, DateStamp) VALUES (" +
                                            fields[0] + ", " +
                                            fields[1] + ",'" +
                                            reachName + "', '" +
                                            fields[2] + "', " +         //parameter name
                                            fields[3] + "," +           //R2
                                            fields[4] + ", " +          //Nash Sutcliffe
                                            fields[5] + ", " +          // log(NS)
                                            fields[6] + ", " +          //RMSE
                                            fields[8] + ", " +          //AD
                                            fields[9] + ", " +          //VR
                                            fields[13]  + ", " +        //KGE
                                            fields[14] + ", " +          //CAT_B
                                            fields[15] + ", " +          //CAT_C
                                            fields[16] + ", " +         //CAT_Ca
                                            fields[17] + ", " +         //CAT_Cb
                                            "Date())";                  //Date 
                                        using (StreamWriter SQLCheck = new StreamWriter("SQLCheck.txt"))
                                        {
                                            SQLCheck.WriteLine(SQLString);
                                        }
                                        OdbcCommand tmp = new OdbcCommand(SQLString, localConnection);
                                        try { tmp.ExecuteNonQuery(); }
                                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                                        tmp.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void executeSQLCommand(string commandString)
        {
            OdbcCommand tmp = new OdbcCommand(commandString, localConnection);
            try { tmp.ExecuteNonQuery(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }
        }

        private void writeParameter(int runID, int ParID, string textValue)
        {
            string insertCommandString;
            string numericValueString;

            try
            {
                double test = Convert.ToDouble(textValue);
                numericValueString = textValue;
            }
            catch { numericValueString = "NULL"; }

            insertCommandString = "INSERT INTO ParList (RunID, ParID, TextValue, NumericValue) VALUES (" 
                + runID.ToString() + ", " + ParID.ToString() + ",  '" + textValue + "', " + numericValueString + ");";
            OdbcCommand tmp = new OdbcCommand(insertCommandString, localConnection);
            //Console.WriteLine(insertCommandString);
            try { tmp.ExecuteNonQuery(); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            tmp.Dispose();
        }

        private void writeParameterName(int ParID, string parName)
        {
            string insertCommandString;

            insertCommandString = "INSERT INTO ParNames (ParID, ParName) VALUES (" +  ParID.ToString() + ",  '" + parName + "');";
            OdbcCommand tmp = new OdbcCommand(insertCommandString, localConnection);
            try { tmp.ExecuteNonQuery(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }
        }

        public void writeParameterSet(int runID, parameterSet pSet)
        {
            Console.WriteLine("Writing parameter set {0}", runID);
            //probably need to open a connection string
            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            int m = 0;
            foreach (ArrayList l in pSet)
            {
                //need to separate out comma-separated lists of land use types
                foreach (parameter p in l)
                {
                    string[] s = (p.stringValue()).Split(MCParameters.separatorChar);
                    foreach (string par in s)
                    {
                        writeParameter(runID, m++, par);
                    }
                }
            }
            localConnection.Close();
        }

        public void writeParameterNames(ParameterArrayList pal)
        {
            //probably need to open a connection string
            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }

            //each row in the header is an integer followed by a field name
            string[] s = (pal.header.ToString()).Split('\n');
            int m = 0;
            foreach (string par in s)
            {
                int splitPos = par.IndexOf(MCParameters.separatorChar);
                string parName = par.Substring(splitPos + 1);
                writeParameterName(m++, parName);
            }
            localConnection.Close();
        }

        public void writeCoefficientWeights()
        {
            //probably need to open a connection string
            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + MCParameters.databaseFileName + ";Uid=;Pwd=;";
            localConnection.ConnectionString = localConnectionString;
            try { localConnection.Open(); }
            catch (OdbcException ex) { Console.WriteLine(ex.Message); }

            using (StreamReader coefficientWeights = new StreamReader(MCParameters.coefficientsWeightFile))
            {
                string line;
                string SQLString;

                while ((line = coefficientWeights.ReadLine()) != null)
                {
                    string[] fields = line.Split(MCParameters.separatorChar);
                    SQLString = "INSERT INTO CoefficientWeights (CoefficientName,CoefficientWeight) VALUES ('" +
                    fields[0] + "', " + fields[1] + ");";

                    OdbcCommand tmp = new OdbcCommand(SQLString, localConnection);
                    try { tmp.ExecuteNonQuery(); }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    tmp.Dispose();
                }
            }
            localConnection.Close();
        }
    }
}
