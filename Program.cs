using Dynastream.Fit;
using FitDemo;
using System;
using System.Diagnostics;
using System.Text;

static class Program
{
    static void Main(string[] arg)
    {
        var baseFile = Directory.GetFiles(Path.Join(Environment.CurrentDirectory, "fit\\base"));
        var additionFile = Directory.GetFiles(Path.Join(Environment.CurrentDirectory, "fit\\addition"));
        var baseMesg = ReadFit(baseFile[0]);
        var additionMesg = ReadFit(additionFile[0]);


        var newFit = CalcNewFit(baseMesg, additionMesg);


        if (!Directory.Exists(Path.Join(Environment.CurrentDirectory, "fit\\new")))
        {
            Directory.CreateDirectory(Path.Join(Environment.CurrentDirectory, "fit\\new"));
        }

        CreateFit(Path.Join(Environment.CurrentDirectory, "fit\\new\\new.fit"), newFit);

        //ShowFit(baseFile[0]);
        //ShowFit(additionFile[0]);
        ShowFit(Path.Join(Environment.CurrentDirectory, "fit\\new\\new.fit"));

    }
    static FiteFileModel CalcNewFit(FiteFileModel baseFit, FiteFileModel additionFit)
    {
        var result = new FiteFileModel();
        ///activity
        result.ActivityFileMsg = new Mesg(baseFit.ActivityFileMsg);

        var time1 = baseFit.ActivityFileMsg.GetFieldValue<System.Single>("TotalTimerTime");
        var time2 = additionFit.ActivityFileMsg.GetFieldValue<System.Single>("TotalTimerTime");
        var totalTimerTime = time1 + time2;
        result.ActivityFileMsg.SetFieldValue<System.Single>("TotalTimerTime", totalTimerTime);

        ///DeviceInfo
        result.DeviceInfoMsg = baseFit.DeviceInfoMsg.Select(d => new Mesg(d)).ToList();

        ///FileId
        result.FileIdMsg = new Mesg(baseFit.FileIdMsg);
        ///UserProfile
        result.UserProfileMsg = new Mesg(baseFit.UserProfileMsg);
        ///Software
        result.SoftwareMsg = baseFit.SoftwareMsg.Select(s => new Mesg(s)).ToList();
        result.SoftwareMsg.AddRange(additionFit.SoftwareMsg.Select(s => new Mesg(s)));
        ///Record
        result.RecordMsg = baseFit.RecordMsg.Select(r => new Mesg(r)).ToList();
        result.RecordMsg.AddRange(additionFit.RecordMsg.Select(r => new Mesg(r)));

        ///Session
        result.SessionMsg = SessionProcesser.Calc(baseFit, additionFit);
        return result;
    }


    static void CreateFit(string fileName, FiteFileModel newFit)
    {



        using var stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

        Encode encoder = new Encode(ProtocolVersion.V20);

        encoder.Open(stream);


        encoder.Write(newFit.ActivityFileMsg);
        encoder.Write(newFit.DeviceInfoMsg);
        encoder.Write(newFit.FileIdMsg);
        encoder.Write(newFit.UserProfileMsg);
        encoder.Write(newFit.SoftwareMsg);
        encoder.Write(newFit.RecordMsg);
        encoder.Write(newFit.SessionMsg);

        encoder.Close();

    }

    static FiteFileModel ReadFit(string fileName)
    {
        var result = new FiteFileModel();
        Console.WriteLine($"file:{fileName}");


        var stream = new FileStream(fileName, FileMode.Open);
        try
        {
            var decode = new Decode();
            var isFit = decode.IsFIT(stream);
            if (!decode.IsFIT(stream))
            {
                Console.WriteLine("not fit file");
                return null;
            }
            decode.MesgEvent += (sender, e) =>
            {
                switch (e.mesg.Name)
                {
                    case "Software":
                        result.SoftwareMsg ??= new List<Mesg>();
                        result.SoftwareMsg.Add(e.mesg);
                        break;
                    case "Record":
                        result.RecordMsg ??= new List<Mesg>();
                        result.RecordMsg.Add(e.mesg);
                        break;
                    case "DeviceInfo":
                        result.DeviceInfoMsg ??= new List<Mesg>();
                        result.DeviceInfoMsg.Add(e.mesg);
                        break;
                    case "UserProfile":
                        result.UserProfileMsg = e.mesg;
                        break;
                    case "FileId":
                        result.FileIdMsg = e.mesg;
                        break;
                    case "Session":
                        result.SessionMsg ??= new List<Mesg>();
                        result.SessionMsg.Add(e.mesg);
                        break;
                    case "Activity":
                        result.ActivityFileMsg = e.mesg;
                        break;
                    default:
                        Console.WriteLine(e.mesg.Name);
                        break;
                }
            };



            if (!decode.Read(stream))
            {
                Console.WriteLine("cant read");
            }
        }
        finally
        {
            stream.Dispose();
            stream.Close();
        }

        return result;
    }


    static void BindEvent(MesgBroadcaster mesgBroadcaster)
    {
        mesgBroadcaster.FileCreatorMesgEvent += FitDemo.Processor.FileCreatorProcesser.Event;
        mesgBroadcaster.TimestampCorrelationMesgEvent += FitDemo.Processor.TimestampCorrelationProcesser.Event;

        mesgBroadcaster.SlaveDeviceMesgEvent += FitDemo.Processor.SlaveDeviceProcesser.Event;
        mesgBroadcaster.CapabilitiesMesgEvent += FitDemo.Processor.CapabilitiesProcesser.Event;
        mesgBroadcaster.FileCapabilitiesMesgEvent += FitDemo.Processor.FileCapabilitiesProcesser.Event;
        mesgBroadcaster.MesgCapabilitiesMesgEvent += FitDemo.Processor.MesgCapabilitiesProcesser.Event;
        mesgBroadcaster.FieldCapabilitiesMesgEvent += FitDemo.Processor.FieldCapabilitiesProcesser.Event;
        mesgBroadcaster.DeviceSettingsMesgEvent += FitDemo.Processor.DeviceSettingsProcesser.Event;
        mesgBroadcaster.HrmProfileMesgEvent += FitDemo.Processor.HrmProfileProcesser.Event;
        mesgBroadcaster.SdmProfileMesgEvent += FitDemo.Processor.SdmProfileProcesser.Event;
        mesgBroadcaster.BikeProfileMesgEvent += FitDemo.Processor.BikeProfileProcesser.Event;
        mesgBroadcaster.ConnectivityMesgEvent += FitDemo.Processor.ConnectivityProcesser.Event;
        mesgBroadcaster.WatchfaceSettingsMesgEvent += FitDemo.Processor.WatchfaceSettingsProcesser.Event;
        mesgBroadcaster.OhrSettingsMesgEvent += FitDemo.Processor.OhrSettingsProcesser.Event;
        mesgBroadcaster.TimeInZoneMesgEvent += FitDemo.Processor.TimeInZoneProcesser.Event;
        mesgBroadcaster.ZonesTargetMesgEvent += FitDemo.Processor.ZonesTargetProcesser.Event;
        mesgBroadcaster.SportMesgEvent += FitDemo.Processor.SportProcesser.Event;
        mesgBroadcaster.HrZoneMesgEvent += FitDemo.Processor.HrZoneProcesser.Event;
        mesgBroadcaster.SpeedZoneMesgEvent += FitDemo.Processor.SpeedZoneProcesser.Event;
        mesgBroadcaster.CadenceZoneMesgEvent += FitDemo.Processor.CadenceZoneProcesser.Event;
        mesgBroadcaster.PowerZoneMesgEvent += FitDemo.Processor.PowerZoneProcesser.Event;
        mesgBroadcaster.MetZoneMesgEvent += FitDemo.Processor.MetZoneProcesser.Event;
        mesgBroadcaster.DiveSettingsMesgEvent += FitDemo.Processor.DiveSettingsProcesser.Event;
        mesgBroadcaster.DiveAlarmMesgEvent += FitDemo.Processor.DiveAlarmProcesser.Event;
        mesgBroadcaster.DiveApneaAlarmMesgEvent += FitDemo.Processor.DiveApneaAlarmProcesser.Event;
        mesgBroadcaster.DiveGasMesgEvent += FitDemo.Processor.DiveGasProcesser.Event;
        mesgBroadcaster.GoalMesgEvent += FitDemo.Processor.GoalProcesser.Event;

        mesgBroadcaster.LapMesgEvent += FitDemo.Processor.LapProcesser.Event;
        mesgBroadcaster.LengthMesgEvent += FitDemo.Processor.LengthProcesser.Event;

        mesgBroadcaster.EventMesgEvent += FitDemo.Processor.EventProcesser.Event;

        mesgBroadcaster.DeviceAuxBatteryInfoMesgEvent += FitDemo.Processor.DeviceAuxBatteryInfoProcesser.Event;
        mesgBroadcaster.TrainingFileMesgEvent += FitDemo.Processor.TrainingFileProcesser.Event;
        mesgBroadcaster.WeatherConditionsMesgEvent += FitDemo.Processor.WeatherConditionsProcesser.Event;
        mesgBroadcaster.WeatherAlertMesgEvent += FitDemo.Processor.WeatherAlertProcesser.Event;
        mesgBroadcaster.GpsMetadataMesgEvent += FitDemo.Processor.GpsMetadataProcesser.Event;
        mesgBroadcaster.CameraEventMesgEvent += FitDemo.Processor.CameraEventProcesser.Event;
        mesgBroadcaster.GyroscopeDataMesgEvent += FitDemo.Processor.GyroscopeDataProcesser.Event;
        mesgBroadcaster.AccelerometerDataMesgEvent += FitDemo.Processor.AccelerometerDataProcesser.Event;
        mesgBroadcaster.MagnetometerDataMesgEvent += FitDemo.Processor.MagnetometerDataProcesser.Event;
        mesgBroadcaster.BarometerDataMesgEvent += FitDemo.Processor.BarometerDataProcesser.Event;
        mesgBroadcaster.ThreeDSensorCalibrationMesgEvent += FitDemo.Processor.ThreeDSensorCalibrationProcesser.Event;
        mesgBroadcaster.OneDSensorCalibrationMesgEvent += FitDemo.Processor.OneDSensorCalibrationProcesser.Event;
        mesgBroadcaster.VideoFrameMesgEvent += FitDemo.Processor.VideoFrameProcesser.Event;
        mesgBroadcaster.ObdiiDataMesgEvent += FitDemo.Processor.ObdiiDataProcesser.Event;
        mesgBroadcaster.NmeaSentenceMesgEvent += FitDemo.Processor.NmeaSentenceProcesser.Event;
        mesgBroadcaster.AviationAttitudeMesgEvent += FitDemo.Processor.AviationAttitudeProcesser.Event;
        mesgBroadcaster.VideoMesgEvent += FitDemo.Processor.VideoProcesser.Event;
        mesgBroadcaster.VideoTitleMesgEvent += FitDemo.Processor.VideoTitleProcesser.Event;
        mesgBroadcaster.VideoDescriptionMesgEvent += FitDemo.Processor.VideoDescriptionProcesser.Event;
        mesgBroadcaster.VideoClipMesgEvent += FitDemo.Processor.VideoClipProcesser.Event;
        mesgBroadcaster.SetMesgEvent += FitDemo.Processor.SetProcesser.Event;
        mesgBroadcaster.JumpMesgEvent += FitDemo.Processor.JumpProcesser.Event;
        mesgBroadcaster.SplitMesgEvent += FitDemo.Processor.SplitProcesser.Event;
        mesgBroadcaster.ClimbProMesgEvent += FitDemo.Processor.ClimbProProcesser.Event;
        mesgBroadcaster.FieldDescriptionMesgEvent += FitDemo.Processor.FieldDescriptionProcesser.Event;
        mesgBroadcaster.DeveloperDataIdMesgEvent += FitDemo.Processor.DeveloperDataIdProcesser.Event;
        mesgBroadcaster.CourseMesgEvent += FitDemo.Processor.CourseProcesser.Event;
        mesgBroadcaster.CoursePointMesgEvent += FitDemo.Processor.CoursePointProcesser.Event;
        mesgBroadcaster.SegmentIdMesgEvent += FitDemo.Processor.SegmentIdProcesser.Event;
        mesgBroadcaster.SegmentLeaderboardEntryMesgEvent += FitDemo.Processor.SegmentLeaderboardEntryProcesser.Event;
        mesgBroadcaster.SegmentPointMesgEvent += FitDemo.Processor.SegmentPointProcesser.Event;
        mesgBroadcaster.SegmentLapMesgEvent += FitDemo.Processor.SegmentLapProcesser.Event;
        mesgBroadcaster.SegmentFileMesgEvent += FitDemo.Processor.SegmentFileProcesser.Event;
        mesgBroadcaster.WorkoutMesgEvent += FitDemo.Processor.WorkoutProcesser.Event;
        mesgBroadcaster.WorkoutSessionMesgEvent += FitDemo.Processor.WorkoutSessionProcesser.Event;
        mesgBroadcaster.WorkoutStepMesgEvent += FitDemo.Processor.WorkoutStepProcesser.Event;
        mesgBroadcaster.ExerciseTitleMesgEvent += FitDemo.Processor.ExerciseTitleProcesser.Event;
        mesgBroadcaster.ScheduleMesgEvent += FitDemo.Processor.ScheduleProcesser.Event;
        mesgBroadcaster.TotalsMesgEvent += FitDemo.Processor.TotalsProcesser.Event;
        mesgBroadcaster.WeightScaleMesgEvent += FitDemo.Processor.WeightScaleProcesser.Event;
        mesgBroadcaster.BloodPressureMesgEvent += FitDemo.Processor.BloodPressureProcesser.Event;
        mesgBroadcaster.MonitoringInfoMesgEvent += FitDemo.Processor.MonitoringInfoProcesser.Event;
        mesgBroadcaster.MonitoringMesgEvent += FitDemo.Processor.MonitoringProcesser.Event;
        mesgBroadcaster.MonitoringHrDataMesgEvent += FitDemo.Processor.MonitoringHrDataProcesser.Event;
        mesgBroadcaster.Spo2DataMesgEvent += FitDemo.Processor.Spo2DataProcesser.Event;
        mesgBroadcaster.HrMesgEvent += FitDemo.Processor.HrProcesser.Event;
        mesgBroadcaster.StressLevelMesgEvent += FitDemo.Processor.StressLevelProcesser.Event;
        mesgBroadcaster.MaxMetDataMesgEvent += FitDemo.Processor.MaxMetDataProcesser.Event;
        mesgBroadcaster.MemoGlobMesgEvent += FitDemo.Processor.MemoGlobProcesser.Event;
        mesgBroadcaster.SleepLevelMesgEvent += FitDemo.Processor.SleepLevelProcesser.Event;
        mesgBroadcaster.AntChannelIdMesgEvent += FitDemo.Processor.AntChannelIdProcesser.Event;
        mesgBroadcaster.AntRxMesgEvent += FitDemo.Processor.AntRxProcesser.Event;
        mesgBroadcaster.AntTxMesgEvent += FitDemo.Processor.AntTxProcesser.Event;
        mesgBroadcaster.ExdScreenConfigurationMesgEvent += FitDemo.Processor.ExdScreenConfigurationProcesser.Event;
        mesgBroadcaster.ExdDataFieldConfigurationMesgEvent += FitDemo.Processor.ExdDataFieldConfigurationProcesser.Event;
        mesgBroadcaster.ExdDataConceptConfigurationMesgEvent += FitDemo.Processor.ExdDataConceptConfigurationProcesser.Event;
        mesgBroadcaster.DiveSummaryMesgEvent += FitDemo.Processor.DiveSummaryProcesser.Event;
        mesgBroadcaster.HrvMesgEvent += FitDemo.Processor.HrvProcesser.Event;
        mesgBroadcaster.BeatIntervalsMesgEvent += FitDemo.Processor.BeatIntervalsProcesser.Event;
        mesgBroadcaster.HrvStatusSummaryMesgEvent += FitDemo.Processor.HrvStatusSummaryProcesser.Event;
        mesgBroadcaster.HrvValueMesgEvent += FitDemo.Processor.HrvValueProcesser.Event;
        mesgBroadcaster.RespirationRateMesgEvent += FitDemo.Processor.RespirationRateProcesser.Event;
        mesgBroadcaster.TankUpdateMesgEvent += FitDemo.Processor.TankUpdateProcesser.Event;
        mesgBroadcaster.TankSummaryMesgEvent += FitDemo.Processor.TankSummaryProcesser.Event;
        mesgBroadcaster.SleepAssessmentMesgEvent += FitDemo.Processor.SleepAssessmentProcesser.Event;
        mesgBroadcaster.PadMesgEvent += FitDemo.Processor.PadProcesser.Event;
    }

    static void ShowFit(string fileName)
    {

        var stream = new FileStream(fileName, FileMode.Open);
        try
        {
            var decode = new Decode();
            var isFit = decode.IsFIT(stream);
            if (!decode.IsFIT(stream))
            {
                Console.WriteLine("not fit file");
                return;
            }


            MesgBroadcaster mesgBroadcaster = new MesgBroadcaster();

            //mesgBroadcaster.ActivityMesgEvent += ActivityProcesser.Event;
            //mesgBroadcaster.DeviceInfoMesgEvent += DeviceInfoProcesser.Event;
            //mesgBroadcaster.FileIdMesgEvent += FileIdProcesser.Event;
            //mesgBroadcaster.UserProfileMesgEvent += UserProfileProcesser.Event;
            //mesgBroadcaster.SoftwareMesgEvent += SoftwareProcesser.Event;
            //mesgBroadcaster.RecordMesgEvent += RecordProcesser.Event;
            mesgBroadcaster.SessionMesgEvent += SessionProcesser.Event;

            ///unknow
            BindEvent(mesgBroadcaster);

            //-------------------------------------------------------------------------------------

            decode.MesgEvent += mesgBroadcaster.OnMesg;

            if (!decode.Read(stream))
            {
                Console.WriteLine("cant read");
            }
        }
        finally
        {
            stream.Dispose();
            stream.Close();
        }
    }
}