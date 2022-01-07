            int i,j;
            header = new StringBuilder();
            header.Append("ParameterSetName").Append(separatorChar);
            for(j=0; j<landUseClasses; j++) header.Append("LandCoverAbbreviation(").Append(j.ToString()).Append(")").Append(separatorChar);
            for(j=0; j<landUseClasses; j++) header.Append("LandCoverName(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("DirectRunoffInitialFlow(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerInitialFlow(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerInitialFlow(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("DirectRunoffInitialPDC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerInitialSOC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerInitialDOC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerInitialDIC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerInitialSOC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerInitialDOC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerInitialDIC(").Append(j.ToString()).Append(")").Append(separatorChar);
header.Append("startDate").Append(separatorChar);
header.Append("numberOfDays").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerPDCToSOC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerPDCToDIC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerPDCToDOC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerSOCToDOC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerSOCToDIC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerDOCToSOC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerDOCToDIC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerSOCToDOC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerSOCToDIC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerDOCToSOC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerDOCToDIC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerCO2MassTransferVelocity(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("DailyPDCInputToOrganicLayer(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("FertilizerAdditionStartDay(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("FertilizerAdditionDuration(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("FertilizerAdditionAmount(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MaximumSoilMoistureDeficit(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("SoilTDiffBnSummerAndWinterMax(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("SnowDepthFactor(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("COUP_10DegreeResponse(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("COUP_BaseT(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("SnowPackInitialDepth(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("SnowPackDegreeDayMeltFactor(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("SnowPackWaterEquivalent(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("SoilTThermalConductivity(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("SoilTSpecificHeatCapacity(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MinimumOrganicLayerFlow(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MinimumMineralLayerFlow(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerMaxDIC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerMaxDIC(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerCO2MassTransferVelocity(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerReturnTime(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerEnzymaticLatchRate(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerCriticalSMD(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerReturnTime(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerEnzymaticLatchRate(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerCriticalSMD(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerB1(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerB2(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerB1(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerB2(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("DirectRunoffResidenceTime(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerResidenceTime(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerResidenceTime(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("OrganicLayerRetentionVolume(").Append(j.ToString()).Append(")").Append(separatorChar);
for(j=0; j<landUseClasses; j++) header.Append("MineralLayerRetentionVolume(").Append(j.ToString()).Append(")").Append(separatorChar);
header.Append("RiverFlow").Append(separatorChar);
header.Append("InitialOpenWaterPDC").Append(separatorChar);
header.Append("InitialOpenWaterDOC").Append(separatorChar);
header.Append("InitialOpenWaterDIC").Append(separatorChar);
header.Append("MinimumWaterTemperature").Append(separatorChar);
header.Append("numberOfReaches").Append(separatorChar);
for (i=0 i<numberOfReaches; i++)
{
header.Append("reachName, 
header.Append("reachLength
header.Append("averageWidth 
header.Append("latitude
header.Append("longitude 
header.Append("flow_a
header.Append("flow_b
header.Append("ReachArea
header.Append("OpenWaterPDCSettlingVelocity
header.Append("DOCToDICSelfShadingFactor
header.Append("DOCToDICRadiationMultiplier
header.Append("OpenWaterDOCToDICMicrobial
header.Append("OpenWaterPDCToDOC
header.Append("OpenWaterDOCToPDC
header.Append("OpenWaterDICMassTransferRate
header.Append("DICToPDCPhotosynthesis
header.Append("AbstractionRate
header.Append("EffluentInputRate
header.Append("EffluentConcentration
header.Append("LitterFallOntoReach
header.Append("AbstractionOnOffSwitch
}
for (i=0 i<numberOfReaches; i++)
CatchmentArea
for(j=0; j<landUseClasses; j++) Land1
BaseFlowIndex
ThresholdSoilZoneFlow
CO2SaturationConcentration
AtmosphericPartialPressureOfCO2
HenryLawConstantForCO2
PDCOverlandExportOffset
SOCOverlandExportOffset
RainfallExcessProportion
MaximumInfiltrationRate
