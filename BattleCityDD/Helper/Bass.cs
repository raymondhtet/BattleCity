using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BattleCity
{
    internal class BassAPI
    {
        #region BASS Native
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Init(int device, int freq, BASSInit flags, IntPtr win, Guid clsid);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Init(int device, int freq, BASSInit flags, IntPtr win, IntPtr clsid);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Free();

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern BASSError BASS_ErrorGetCode();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Pause();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Start();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Stop();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Update(int length);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern float BASS_GetCPU();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_GetInfo([In, Out] BASS_INFO info);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_GetVersion();

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr BASS_GetDSoundObject(int handle);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr BASS_GetDSoundObject(BASSDirectSound dsobject);
        #endregion

        #region BASS 3D Methods
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern void BASS_Apply3D();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Get3DFactors([In, Out, MarshalAs(UnmanagedType.AsAny)] object distf, [In, Out, MarshalAs(UnmanagedType.AsAny)] object rollf, [In, Out, MarshalAs(UnmanagedType.AsAny)] object doppf);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Get3DFactors(ref float distf, ref float rollf, ref float doppf);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Get3DPosition([In, Out] BASS_3DVECTOR pos, [In, Out] BASS_3DVECTOR vel, [In, Out] BASS_3DVECTOR front, [In, Out] BASS_3DVECTOR top);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Set3DFactors(float distf, float rollf, float doppf);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_Set3DPosition([In] BASS_3DVECTOR pos, [In] BASS_3DVECTOR vel, [In] BASS_3DVECTOR front, [In] BASS_3DVECTOR top);
        #endregion

        #region BASS Channel Methods
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern double BASS_ChannelBytes2Seconds(int handle, long pos);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern BASSFlag BASS_ChannelFlags(int handle, BASSFlag flags, BASSFlag mask);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelGet3DAttributes(int handle, ref BASS3DMode mode, ref float min, ref float max, ref int iangle, ref int oangle, ref int outvol);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelGet3DAttributes(int handle, [In, Out, MarshalAs(UnmanagedType.AsAny)] object mode, [In, Out, MarshalAs(UnmanagedType.AsAny)] object min, [In, Out, MarshalAs(UnmanagedType.AsAny)] object max, [In, Out, MarshalAs(UnmanagedType.AsAny)] object iangle, [In, Out, MarshalAs(UnmanagedType.AsAny)] object oangle, [In, Out, MarshalAs(UnmanagedType.AsAny)] object outvol);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelGet3DPosition(int handle, [In, Out] BASS_3DVECTOR pos, [In, Out] BASS_3DVECTOR orient, [In, Out] BASS_3DVECTOR vel);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelGetAttribute(int handle, BASSAttribute attrib, ref float value);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_ChannelGetData(int handle, [In, Out] byte[] buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_ChannelGetData(int handle, [In, Out] short[] buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_ChannelGetData(int handle, [In, Out] int[] buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_ChannelGetData(int handle, [In, Out] float[] buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_ChannelGetData(int handle, IntPtr buffer, int length);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_ChannelGetDevice(int handle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelGetInfo(int handle, [In, Out] ref BASS_CHANNELINFO info);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern long BASS_ChannelGetLength(int handle, BASSMode mode);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_ChannelGetLevel(int handle);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern long BASS_ChannelGetPosition(int handle, BASSMode mode);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr BASS_ChannelGetTags(int handle, BASSTag tags);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern BASSActive BASS_ChannelIsActive(int handle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelIsSliding(int handle, BASSAttribute attrib);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelLock(int handle, [MarshalAs(UnmanagedType.Bool)] bool state);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelPause(int handle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelPlay(int handle, [MarshalAs(UnmanagedType.Bool)] bool restart);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelRemoveDSP(int handle, int dsp);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelRemoveFX(int handle, int fx);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelRemoveLink(int handle, int chan);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelRemoveSync(int handle, int sync);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern long BASS_ChannelSeconds2Bytes(int handle, double pos);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelSet3DAttributes(int handle, BASS3DMode mode, float min, float max, int iangle, int oangle, int outvol);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelSet3DPosition(int handle, [In] BASS_3DVECTOR pos, [In] BASS_3DVECTOR orient, [In] BASS_3DVECTOR vel);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelSetAttribute(int handle, BASSAttribute attrib, float value);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelSetDevice(int handle, int device);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_ChannelSetDSP(int handle, DSPPROC proc, IntPtr user, int priority);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_ChannelSetFX(int handle, BASSFXType type, int priority);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelSetLink(int handle, int chan);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelSetPosition(int handle, long pos, BASSMode mode);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_ChannelSetSync(int handle, BASSSync type, long param, SYNCPROC proc, IntPtr user);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelSlideAttribute(int handle, BASSAttribute attrib, float value, int time);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelStop(int handle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_ChannelUpdate(int handle, int length);
        #endregion

        #region BASS Config Methods
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_GetConfig(BASSConfig option);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", EntryPoint = "BASS_GetConfig", CharSet = CharSet.Auto)]
        public static extern bool BASS_GetConfigBool(BASSConfig option);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr BASS_GetConfigPtr(BASSConfig option);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SetConfig(BASSConfig option, [In, MarshalAs(UnmanagedType.Bool)] bool newvalue);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SetConfig(BASSConfig option, int newvalue);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SetConfigPtr(BASSConfig option, IntPtr newvalue);
        #endregion

        #region BASS Device Methods
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SetDevice(int device);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_GetDevice();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_GetDeviceInfo([In] int handle, [In, Out] ref BASS_DEVICEINFO info);
        #endregion

        #region BASS EAX Methods
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_GetEAXParameters([In, Out, MarshalAs(UnmanagedType.AsAny)] object env, [In, Out, MarshalAs(UnmanagedType.AsAny)] object vol, [In, Out, MarshalAs(UnmanagedType.AsAny)] object decay, [In, Out, MarshalAs(UnmanagedType.AsAny)] object damp);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_GetEAXParameters(ref EAXEnvironment env, ref float vol, ref float decay, ref float damp);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SetEAXParameters(EAXEnvironment env, float vol, float decay, float damp);
        #endregion

        #region BASS Fx Methods
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_FXGetParameters(int handle, [In, Out, MarshalAs(UnmanagedType.AsAny)] object para);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_FXReset(int handle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_FXSetParameters(int handle, [In, MarshalAs(UnmanagedType.AsAny)] object para);
        #endregion

        #region BASS Record Methods
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_RecordFree();

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_RecordGetDevice();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_RecordGetDeviceInfo([In] int handle, [In, Out] ref BASS_DEVICEINFO info);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_RecordGetInfo([In, Out] BASS_RECORDINFO info);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_RecordGetInput(int input, ref float volume);

        [DllImport("bass.dll", EntryPoint = "BASS_RecordGetInputName", CharSet = CharSet.Auto)]
        public static extern IntPtr BASS_RecordGetInputNamePtr(int input);

        [DllImport("bass.dll", EntryPoint = "BASS_RecordGetInput", CharSet = CharSet.Auto)]
        public static extern int BASS_RecordGetInputPtr(int input, IntPtr user);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_RecordInit(int device);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_RecordSetDevice(int device);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_RecordSetInput(int input, BASSInput setting, float volume);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_RecordStart(int freq, int chans, BASSFlag flags, RECORDPROC proc, IntPtr user);
        #endregion

        #region BASS Sample Methods
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_SampleCreate(int length, int freq, int chans, int max, BASSFlag flags);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleFree(int handle);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_SampleGetChannel(int handle, [MarshalAs(UnmanagedType.Bool)] bool onlynew);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_SampleGetChannels(int handle, int[] channels);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleGetData(int handle, byte[] buffer);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleGetData(int handle, short[] buffer);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleGetData(int handle, int[] buffer);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleGetData(int handle, float[] buffer);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleGetData(int handle, IntPtr buffer);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleGetInfo(int handle, [In, Out] BASS_SAMPLE info);

        [DllImport("bass.dll", EntryPoint = "BASS_SampleLoad", CharSet = CharSet.Auto)]
        public static extern int BASS_SampleLoadMemory([MarshalAs(UnmanagedType.Bool)] bool mem, byte[] buffer, long offset, int length, int max, BASSFlag flags);

        [DllImport("bass.dll", EntryPoint = "BASS_SampleLoad", CharSet = CharSet.Auto)]
        public static extern int BASS_SampleLoadMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr buffer, long offset, int length, int max, BASSFlag flags);

        [DllImport("bass.dll", EntryPoint = "BASS_SampleLoad", CharSet = CharSet.Auto)]
        public static extern int BASS_SampleLoadUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [In, MarshalAs(UnmanagedType.LPWStr)] string filename, long offset, int length, int max, BASSFlag flags);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleSetData(int handle, IntPtr buffer);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleSetData(int handle, byte[] buffer);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleSetData(int handle, short[] buffer);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleSetData(int handle, int[] buffer);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleSetData(int handle, float[] buffer);
        
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleSetInfo(int handle, [In] BASS_SAMPLE info);
        
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SampleStop(int handle);
        #endregion
        
        #region BASS Stream Methods
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamCreate(int freq, int chans, BASSFlag flags, STREAMPROC proc, IntPtr user);

        [DllImport("bass.dll", EntryPoint = "BASS_StreamCreateFile", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamCreateFileMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr handle, long offset, long length, BASSFlag flags);
        
        [DllImport("bass.dll", EntryPoint = "BASS_StreamCreateFile", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamCreateFileUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [In, MarshalAs(UnmanagedType.LPWStr)] string fileName, long offset, long length, BASSFlag flags);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamCreateFileUser(BASSStreamSystem system, BASSFlag flags, BASS_FILEPROCS procs, IntPtr user);
        
        [DllImport("bass.dll", EntryPoint = "BASS_StreamCreate", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamCreatePtr(int freq, int chans, BASSFlag flags, IntPtr proc, IntPtr user);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamCreateURL([In, MarshalAs(UnmanagedType.LPStr)] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_StreamFree(int handle);
        
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern long BASS_StreamGetFilePosition(int handle, BASSStreamFilePosition mode);
        
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamPutData(int handle, byte[] buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamPutData(int handle, short[] buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamPutData(int handle, int[] buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamPutData(int handle, IntPtr buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamPutData(int handle, float[] buffer, int length);
        
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamPutFileData(int handle, byte[] buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamPutFileData(int handle, short[] buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamPutFileData(int handle, int[] buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamPutFileData(int handle, IntPtr buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern int BASS_StreamPutFileData(int handle, float[] buffer, int length);
        #endregion

        #region BASS Volume Methods
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern float BASS_GetVolume();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_SetVolume(float volume);
        #endregion

        #region BASS Music Methods
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_MusicFree(int handle);

        [DllImport("bass.dll", EntryPoint = "BASS_MusicLoad", CharSet = CharSet.Auto)]
        public static extern int BASS_MusicLoadMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr buffer, long offset, int length, BASSFlag flags, int freq);
        [DllImport("bass.dll", EntryPoint = "BASS_MusicLoad", CharSet = CharSet.Auto)]
        public static extern int BASS_MusicLoadMemory([MarshalAs(UnmanagedType.Bool)] bool mem, byte[] buffer, long offset, int length, BASSFlag flags, int freq);

        [DllImport("bass.dll", EntryPoint = "BASS_MusicLoad", CharSet = CharSet.Auto)]
        public static extern int BASS_MusicLoadUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [In, MarshalAs(UnmanagedType.LPWStr)] string fileName, long offset, int length, BASSFlag flags, int freq);
        #endregion

        #region BASS Plugin Methods
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        public static extern bool BASS_PluginFree(int handle);

        [DllImport("bass.dll", EntryPoint = "BASS_PluginGetInfo", CharSet = CharSet.Auto)]
        public static extern IntPtr BASS_PluginGetInfoPtr(int handle);

        [DllImport("bass.dll", EntryPoint = "BASS_PluginLoad", CharSet = CharSet.Auto)]
        public static extern int BASS_PluginLoadUnicode([In, MarshalAs(UnmanagedType.LPWStr)] string filename, BASSFlag flags);

        public static BASS_PLUGININFO BASS_PluginGetInfo(int handle)
        {
            if (handle != 0)
            {
                IntPtr ptr = BASS_PluginGetInfoPtr(handle);
                if (ptr != IntPtr.Zero)
                {
                    return (BASS_PLUGININFO)Marshal.PtrToStructure(ptr, typeof(BASS_PLUGININFO));
                }
            }
            return null;
        }
        #endregion

        #region Enumeric
        public enum BASS3DAlgorithm
        {
            BASS_3DALG_DEFAULT,
            BASS_3DALG_OFF,
            BASS_3DALG_FULL,
            BASS_3DALG_LIGHT
        }

        public enum BASS3DMode
        {
            BASS_3DMODE_LEAVECURRENT = -1,
            BASS_3DMODE_NORMAL = 0,
            BASS_3DMODE_OFF = 2,
            BASS_3DMODE_RELATIVE = 1
        }

        public enum BASSActive
        {
            BASS_ACTIVE_STOPPED,
            BASS_ACTIVE_PLAYING,
            BASS_ACTIVE_STALLED,
            BASS_ACTIVE_PAUSED
        }

        public enum BASSAttribute
        {
            BASS_ATTRIB_EAXMIX = 4,
            BASS_ATTRIB_FREQ = 1,
            BASS_ATTRIB_MIDI_PPQN = 0x12000,
            BASS_ATTRIB_MIDI_TRACK_VOL = 0x12100,
            BASS_ATTRIB_MUSIC_AMPLIFY = 0x100,
            BASS_ATTRIB_MUSIC_BPM = 0x103,
            BASS_ATTRIB_MUSIC_PANSEP = 0x101,
            BASS_ATTRIB_MUSIC_PSCALER = 0x102,
            BASS_ATTRIB_MUSIC_SPEED = 260,
            BASS_ATTRIB_MUSIC_VOL_CHAN = 0x200,
            BASS_ATTRIB_MUSIC_VOL_GLOBAL = 0x105,
            BASS_ATTRIB_MUSIC_VOL_INST = 0x300,
            BASS_ATTRIB_PAN = 3,
            BASS_ATTRIB_REVERSE_DIR = 0x11000,
            BASS_ATTRIB_TEMPO = 0x10000,
            BASS_ATTRIB_TEMPO_FREQ = 0x10002,
            BASS_ATTRIB_TEMPO_OPTION_AA_FILTER_LENGTH = 0x10011,
            BASS_ATTRIB_TEMPO_OPTION_OVERLAP_MS = 0x10015,
            BASS_ATTRIB_TEMPO_OPTION_PREVENT_CLICK = 0x10016,
            BASS_ATTRIB_TEMPO_OPTION_SEEKWINDOW_MS = 0x10014,
            BASS_ATTRIB_TEMPO_OPTION_SEQUENCE_MS = 0x10013,
            BASS_ATTRIB_TEMPO_OPTION_USE_AA_FILTER = 0x10010,
            BASS_ATTRIB_TEMPO_OPTION_USE_QUICKALGO = 0x10012,
            BASS_ATTRIB_TEMPO_PITCH = 0x10001,
            BASS_ATTRIB_VOL = 2
        }

        [Flags]
        public enum BASSChannelType
        {
            BASS_CTYPE_MUSIC_IT = 0x20004,
            BASS_CTYPE_MUSIC_MO3 = 0x100,
            BASS_CTYPE_MUSIC_MOD = 0x20000,
            BASS_CTYPE_MUSIC_MTM = 0x20001,
            BASS_CTYPE_MUSIC_S3M = 0x20002,
            BASS_CTYPE_MUSIC_XM = 0x20003,
            BASS_CTYPE_RECORD = 2,
            BASS_CTYPE_SAMPLE = 1,
            BASS_CTYPE_STREAM = 0x10000,
            BASS_CTYPE_STREAM_AAC = 0x10b00,
            BASS_CTYPE_STREAM_AC3 = 0x11000,
            BASS_CTYPE_STREAM_ADX = 0x1f000,
            BASS_CTYPE_STREAM_AIFF = 0x10006,
            BASS_CTYPE_STREAM_ALAC = 0x10e00,
            BASS_CTYPE_STREAM_APE = 0x10700,
            BASS_CTYPE_STREAM_CD = 0x10200,
            BASS_CTYPE_STREAM_FLAC = 0x10900,
            BASS_CTYPE_STREAM_MIDI = 0x10d00,
            BASS_CTYPE_STREAM_MIXER = 0x10800,
            BASS_CTYPE_STREAM_MP1 = 0x10003,
            BASS_CTYPE_STREAM_MP2 = 0x10004,
            BASS_CTYPE_STREAM_MP3 = 0x10005,
            BASS_CTYPE_STREAM_MP4 = 0x10b01,
            BASS_CTYPE_STREAM_MPC = 0x10a00,
            BASS_CTYPE_STREAM_OFR = 0x10600,
            BASS_CTYPE_STREAM_OGG = 0x10002,
            BASS_CTYPE_STREAM_SPLIT = 0x10801,
            BASS_CTYPE_STREAM_SPX = 0x10c00,
            BASS_CTYPE_STREAM_TTA = 0x10f00,
            BASS_CTYPE_STREAM_VIDEO = 0x11100,
            BASS_CTYPE_STREAM_WAV = 0x40000,
            BASS_CTYPE_STREAM_WAV_FLOAT = 0x50003,
            BASS_CTYPE_STREAM_WAV_PCM = 0x50001,
            BASS_CTYPE_STREAM_WINAMP = 0x10400,
            BASS_CTYPE_STREAM_WMA = 0x10300,
            BASS_CTYPE_STREAM_WMA_MP3 = 0x10301,
            BASS_CTYPE_STREAM_WV = 0x10500,
            BASS_CTYPE_STREAM_WV_H = 0x10501,
            BASS_CTYPE_STREAM_WV_L = 0x10502,
            BASS_CTYPE_STREAM_WV_LH = 0x10503,
            BASS_CTYPE_UNKNOWN = 0
        }

        public enum BASSConfig
        {
            BASS_CONFIG_3DALGORITHM = 10,
            BASS_CONFIG_AC3_DYNRNG = 0x10001,
            BASS_CONFIG_BUFFER = 0,
            BASS_CONFIG_CD_AUTOSPEED = 0x10202,
            BASS_CONFIG_CD_FREEOLD = 0x10200,
            BASS_CONFIG_CD_RETRY = 0x10201,
            BASS_CONFIG_CD_SKIPERROR = 0x10203,
            BASS_CONFIG_CURVE_PAN = 8,
            BASS_CONFIG_CURVE_VOL = 7,
            BASS_CONFIG_ENCODE_CAST_PROXY = 0x10311,
            BASS_CONFIG_ENCODE_CAST_TIMEOUT = 0x10310,
            BASS_CONFIG_ENCODE_PRIORITY = 0x10300,
            BASS_CONFIG_FLOATDSP = 9,
            BASS_CONFIG_GVOL_MUSIC = 6,
            BASS_CONFIG_GVOL_SAMPLE = 4,
            BASS_CONFIG_GVOL_STREAM = 5,
            BASS_CONFIG_MIDI_AUTOFONT = 0x10402,
            BASS_CONFIG_MIDI_COMPACT = 0x10400,
            BASS_CONFIG_MIDI_DEFFONT = 0x10403,
            BASS_CONFIG_MIDI_VOICES = 0x10401,
            BASS_CONFIG_MIXER_BUFFER = 0x10601,
            BASS_CONFIG_MIXER_FILTER = 0x10600,
            BASS_CONFIG_MP4_VIDEO = 0x10700,
            BASS_CONFIG_MUSIC_VIRTUAL = 0x16,
            BASS_CONFIG_NET_AGENT = 0x10,
            BASS_CONFIG_NET_BUFFER = 12,
            BASS_CONFIG_NET_PASSIVE = 0x12,
            BASS_CONFIG_NET_PLAYLIST = 0x15,
            BASS_CONFIG_NET_PREBUF = 15,
            BASS_CONFIG_NET_PROXY = 0x11,
            BASS_CONFIG_NET_TIMEOUT = 11,
            BASS_CONFIG_PAUSE_NOPLAY = 13,
            BASS_CONFIG_REC_BUFFER = 0x13,
            BASS_CONFIG_SPLIT_BUFFER = 0x10610,
            BASS_CONFIG_UPDATEPERIOD = 1,
            BASS_CONFIG_UPDATETHREADS = 0x18,
            BASS_CONFIG_VERIFY = 0x17,
            BASS_CONFIG_VIDEO_RENDERER = 0x100,
            BASS_CONFIG_WMA_ASYNC = 0x1010f,
            BASS_CONFIG_WMA_BASSFILE = 0x10103,
            BASS_CONFIG_WMA_NETSEEK = 0x10104,
            BASS_CONFIG_WMA_PREBUF = 0x10101,
            BASS_CONFIG_WMA_VIDEO = 0x10105
        }

        [Flags]
        public enum BASSData
        {
            BASS_DATA_AVAILABLE = 0,
            BASS_DATA_FFT_INDIVIDUAL = 0x10,
            BASS_DATA_FFT_NOWINDOW = 0x20,
            BASS_DATA_FFT1024 = -2147483646,
            BASS_DATA_FFT2048 = -2147483645,
            BASS_DATA_FFT256 = -2147483648,
            BASS_DATA_FFT4096 = -2147483644,
            BASS_DATA_FFT512 = -2147483647,
            BASS_DATA_FFT8192 = -2147483643,
            BASS_DATA_FLOAT = 0x40000000
        }

        [Flags]
        public enum BASSDeviceInfo
        {
            BASS_DEVICE_DEFAULT = 2,
            BASS_DEVICE_ENABLED = 1,
            BASS_DEVICE_INIT = 4,
            BASS_DEVICE_NONE = 0
        }

        public enum BASSDirectSound
        {
            BASS_OBJECT_DS = 1,
            BASS_OBJECT_DS3DL = 2
        }

        public enum BASSError
        {
            BASS_ERROR_ACM_CANCEL = 0x7d0,
            BASS_ERROR_ALREADY = 14,
            BASS_ERROR_BUFLOST = 4,
            BASS_ERROR_CAST_DENIED = 0x834,
            BASS_ERROR_CDTRACK = 13,
            BASS_ERROR_CODEC = 0x2c,
            BASS_ERROR_CREATE = 0x21,
            BASS_ERROR_DECODE = 0x26,
            BASS_ERROR_DEVICE = 0x17,
            BASS_ERROR_DRIVER = 3,
            BASS_ERROR_DX = 0x27,
            BASS_ERROR_EMPTY = 0x1f,
            BASS_ERROR_ENDED = 0x2d,
            BASS_ERROR_FILEFORM = 0x29,
            BASS_ERROR_FILEOPEN = 2,
            BASS_ERROR_FORMAT = 6,
            BASS_ERROR_FREQ = 0x19,
            BASS_ERROR_HANDLE = 5,
            BASS_ERROR_ILLPARAM = 20,
            BASS_ERROR_ILLTYPE = 0x13,
            BASS_ERROR_INIT = 8,
            BASS_ERROR_MEM = 1,
            BASS_ERROR_NO3D = 0x15,
            BASS_ERROR_NOCD = 12,
            BASS_ERROR_NOCHAN = 0x12,
            BASS_ERROR_NOEAX = 0x16,
            BASS_ERROR_NOFX = 0x22,
            BASS_ERROR_NOHW = 0x1d,
            BASS_ERROR_NONET = 0x20,
            BASS_ERROR_NOPAUSE = 0x10,
            BASS_ERROR_NOPLAY = 0x18,
            BASS_ERROR_NOTAUDIO = 0x11,
            BASS_ERROR_NOTAVAIL = 0x25,
            BASS_ERROR_NOTFILE = 0x1b,
            BASS_ERROR_PLAYING = 0x23,
            BASS_ERROR_POSITION = 7,
            BASS_ERROR_SPEAKER = 0x2a,
            BASS_ERROR_START = 9,
            BASS_ERROR_TIMEOUT = 40,
            BASS_ERROR_UNKNOWN = -1,
            BASS_ERROR_VERSION = 0x2b,
            BASS_ERROR_VIDEO_ABORT = 0x2f,
            BASS_ERROR_VIDEO_CANNOT_CONNECT = 0x30,
            BASS_ERROR_VIDEO_CANNOT_READ = 0x31,
            BASS_ERROR_VIDEO_CANNOT_WRITE = 50,
            BASS_ERROR_VIDEO_FAILURE = 0x33,
            BASS_ERROR_VIDEO_FILTER = 0x34,
            BASS_ERROR_VIDEO_INVALID_CHAN = 0x35,
            BASS_ERROR_VIDEO_INVALID_DLL = 0x36,
            BASS_ERROR_VIDEO_INVALID_FORMAT = 0x37,
            BASS_ERROR_VIDEO_INVALID_HANDLE = 0x38,
            BASS_ERROR_VIDEO_INVALID_PARAMETER = 0x39,
            BASS_ERROR_VIDEO_NO_AUDIO = 0x3a,
            BASS_ERROR_VIDEO_NO_EFFECT = 0x3b,
            BASS_ERROR_VIDEO_NO_INTERFACE = 60,
            BASS_ERROR_VIDEO_NO_RENDERER = 0x3d,
            BASS_ERROR_VIDEO_NO_SUPPORT = 0x3e,
            BASS_ERROR_VIDEO_NO_VIDEO = 0x3f,
            BASS_ERROR_VIDEO_NOT_ALLOWED = 0x40,
            BASS_ERROR_VIDEO_NOT_CONNECTED = 0x41,
            BASS_ERROR_VIDEO_NOT_EXISTS = 0x42,
            BASS_ERROR_VIDEO_NOT_FOUND = 0x43,
            BASS_ERROR_VIDEO_NOT_READY = 0x44,
            BASS_ERROR_VIDEO_NULL_DEVICE = 0x45,
            BASS_ERROR_VIDEO_OPEN = 70,
            BASS_ERROR_VIDEO_OUTOFMEMORY = 0x47,
            BASS_ERROR_VIDEO_PARTIAL_RENDER = 0x48,
            BASS_ERROR_VIDEO_TIME_OUT = 0x49,
            BASS_ERROR_VIDEO_UNKNOWN_FILE_TYPE = 0x4a,
            BASS_ERROR_VIDEO_UNSUPPORT_STREAM = 0x4b,
            BASS_ERROR_VIDEO_VIDEO_FILTER = 0x4c,
            BASS_ERROR_WMA_CODEC = 0x3eb,
            BASS_ERROR_WMA_DENIED = 0x3ea,
            BASS_ERROR_WMA_INDIVIDUAL = 0x3ec,
            BASS_ERROR_WMA_LICENSE = 0x3e8,
            BASS_ERROR_WMA_WM9 = 0x3e9,
            BASS_FX_ERROR_BPMINUSE = 0xfa1,
            BASS_FX_ERROR_NODECODE = 0xfa0,
            BASS_OK = 0,
            BASS_VST_ERROR_NOINPUTS = 0xbb8,
            BASS_VST_ERROR_NOOUTPUTS = 0xbb9,
            BASS_VST_ERROR_NOREALTIME = 0xbba
        }

        [Flags]
        public enum BASSFlag
        {
            BASS_AAC_STEREO = 0x400000,
            BASS_AC3_DOWNMIX_2 = 0x200,
            BASS_AC3_DOWNMIX_4 = 0x400,
            BASS_AC3_DOWNMIX_DOLBY = 0x600,
            BASS_AC3_DYNAMIC_RANGE = 0x800,
            BASS_CD_C2ERRORS = 0x800,
            BASS_CD_SUBCHANNEL = 0x200,
            BASS_CD_SUBCHANNEL_NOHW = 0x400,
            BASS_DEFAULT = 0,
            BASS_FX_BPM_BKGRND = 1,
            BASS_FX_BPM_MULT2 = 2,
            BASS_FX_FREESOURCE = 0x10000,
            BASS_MIDI_DECAYEND = 0x1000,
            BASS_MIDI_DECAYSEEK = 0x4000,
            BASS_MIDI_NOFX = 0x2000,
            BASS_MIXER_BUFFER = 0x2000,
            BASS_MIXER_DOWNMIX = 0x400000,
            BASS_MIXER_END = 0x10000,
            BASS_MIXER_FILTER = 0x1000,
            BASS_MIXER_LIMIT = 0x4000,
            BASS_MIXER_MATRIX = 0x10000,
            BASS_MIXER_NONSTOP = 0x20000,
            BASS_MIXER_NORAMPIN = 0x800000,
            BASS_MIXER_PAUSE = 0x20000,
            BASS_MIXER_RESUME = 0x1000,
            BASS_MUSIC_3D = 8,
            BASS_MUSIC_AUTOFREE = 0x40000,
            BASS_MUSIC_DECODE = 0x200000,
            BASS_MUSIC_FLOAT = 0x100,
            BASS_MUSIC_FT2MOD = 0x2000,
            BASS_MUSIC_FX = 0x80,
            BASS_MUSIC_LOOP = 4,
            BASS_MUSIC_MONO = 2,
            BASS_MUSIC_NONINTER = 0x10000,
            BASS_MUSIC_NOSAMPLE = 0x100000,
            BASS_MUSIC_POSRESET = 0x8000,
            BASS_MUSIC_POSRESETEX = 0x400000,
            BASS_MUSIC_PRESCAN = 0x20000,
            BASS_MUSIC_PT1MOD = 0x4000,
            BASS_MUSIC_RAMP = 0x200,
            BASS_MUSIC_RAMPS = 0x400,
            BASS_MUSIC_SINCINTER = 0x800000,
            BASS_MUSIC_STOPBACK = 0x80000,
            BASS_MUSIC_SURROUND = 0x800,
            BASS_MUSIC_SURROUND2 = 0x1000,
            BASS_RECORD_PAUSE = 0x8000,
            BASS_SAMPLE_3D = 8,
            BASS_SAMPLE_8BITS = 1,
            BASS_SAMPLE_FLOAT = 0x100,
            BASS_SAMPLE_FX = 0x80,
            BASS_SAMPLE_LOOP = 4,
            BASS_SAMPLE_MONO = 2,
            BASS_SAMPLE_MUTEMAX = 0x20,
            BASS_SAMPLE_OVER_DIST = 0x30000,
            BASS_SAMPLE_OVER_POS = 0x20000,
            BASS_SAMPLE_OVER_VOL = 0x10000,
            BASS_SAMPLE_SOFTWARE = 0x10,
            BASS_SAMPLE_VAM = 0x40,
            BASS_SPEAKER_CENLFE = 0x3000000,
            BASS_SPEAKER_CENTER = 0x13000000,
            BASS_SPEAKER_FRONT = 0x1000000,
            BASS_SPEAKER_FRONTLEFT = 0x11000000,
            BASS_SPEAKER_FRONTRIGHT = 0x21000000,
            BASS_SPEAKER_LEFT = 0x10000000,
            BASS_SPEAKER_LFE = 0x23000000,
            BASS_SPEAKER_PAIR1 = 0x1000000,
            BASS_SPEAKER_PAIR10 = 0xa000000,
            BASS_SPEAKER_PAIR11 = 0xb000000,
            BASS_SPEAKER_PAIR12 = 0xc000000,
            BASS_SPEAKER_PAIR13 = 0xd000000,
            BASS_SPEAKER_PAIR14 = 0xe000000,
            BASS_SPEAKER_PAIR15 = 0xf000000,
            BASS_SPEAKER_PAIR2 = 0x2000000,
            BASS_SPEAKER_PAIR3 = 0x3000000,
            BASS_SPEAKER_PAIR4 = 0x4000000,
            BASS_SPEAKER_PAIR5 = 0x5000000,
            BASS_SPEAKER_PAIR6 = 0x6000000,
            BASS_SPEAKER_PAIR7 = 0x7000000,
            BASS_SPEAKER_PAIR8 = 0x8000000,
            BASS_SPEAKER_PAIR9 = 0x9000000,
            BASS_SPEAKER_REAR = 0x2000000,
            BASS_SPEAKER_REAR2 = 0x4000000,
            BASS_SPEAKER_REAR2LEFT = 0x14000000,
            BASS_SPEAKER_REAR2RIGHT = 0x24000000,
            BASS_SPEAKER_REARLEFT = 0x12000000,
            BASS_SPEAKER_REARRIGHT = 0x22000000,
            BASS_SPEAKER_RIGHT = 0x20000000,
            BASS_STREAM_AUTOFREE = 0x40000,
            BASS_STREAM_BLOCK = 0x100000,
            BASS_STREAM_DECODE = 0x200000,
            BASS_STREAM_PRESCAN = 0x20000,
            BASS_STREAM_RESTRATE = 0x80000,
            BASS_STREAM_STATUS = 0x800000,
            BASS_UNICODE = -2147483648,
            BASS_VIDEO_AUTO_MOVE = 0x800,
            BASS_VIDEO_AUTO_PAINT = 0x200,
            BASS_VIDEO_AUTO_RESIZE = 0x400,
            BASS_VIDEO_DISABLE_VIDEO = 0x4000,
            BASS_VIDEO_FILTER_CALLBACK = 0x10000,
            BASS_VIDEO_FILTERNAME = 0x1000,
            BASS_VIDEO_VIDEOEFFECT = 0x2000,
            BASS_WV_STEREO = 0x400000
        }

        public enum BASSFXPhase
        {
            BASS_FX_PHASE_NEG_180,
            BASS_FX_PHASE_NEG_90,
            BASS_FX_PHASE_ZERO,
            BASS_FX_PHASE_90,
            BASS_FX_PHASE_180
        }

        public enum BASSFXType
        {
            BASS_FX_BFX_APF = 0x1000e,
            BASS_FX_BFX_AUTOWAH = 0x10009,
            BASS_FX_BFX_CHORUS = 0x1000d,
            BASS_FX_BFX_COMPRESSOR = 0x1000f,
            BASS_FX_BFX_COMPRESSOR2 = 0x10011,
            BASS_FX_BFX_DAMP = 0x10008,
            BASS_FX_BFX_DISTORTION = 0x10010,
            BASS_FX_BFX_ECHO = 0x10001,
            BASS_FX_BFX_ECHO2 = 0x1000a,
            BASS_FX_BFX_ECHO3 = 0x1000c,
            BASS_FX_BFX_FLANGER = 0x10002,
            BASS_FX_BFX_LPF = 0x10006,
            BASS_FX_BFX_MIX = 0x10007,
            BASS_FX_BFX_PEAKEQ = 0x10004,
            BASS_FX_BFX_PHASER = 0x1000b,
            BASS_FX_BFX_REVERB = 0x10005,
            BASS_FX_BFX_ROTATE = 0x10000,
            BASS_FX_BFX_VOLUME = 0x10003,
            BASS_FX_BFX_VOLUME_ENV = 0x10012,
            BASS_FX_DX8_CHORUS = 0,
            BASS_FX_DX8_COMPRESSOR = 1,
            BASS_FX_DX8_DISTORTION = 2,
            BASS_FX_DX8_ECHO = 3,
            BASS_FX_DX8_FLANGER = 4,
            BASS_FX_DX8_GARGLE = 5,
            BASS_FX_DX8_I3DL2REVERB = 6,
            BASS_FX_DX8_PARAMEQ = 7,
            BASS_FX_DX8_REVERB = 8
        }

        [Flags]
        public enum BASSInfo
        {
            DSCAPS_CERTIFIED = 0x40,
            DSCAPS_CONTINUOUSRATE = 0x10,
            DSCAPS_EMULDRIVER = 0x20,
            DSCAPS_NONE = 0,
            DSCAPS_SECONDARY16BIT = 0x800,
            DSCAPS_SECONDARY8BIT = 0x400,
            DSCAPS_SECONDARYMONO = 0x100,
            DSCAPS_SECONDARYSTEREO = 0x200
        }

        [Flags]
        public enum BASSInit
        {
            BASS_DEVICE_3D = 4,
            BASS_DEVICE_8BITS = 1,
            BASS_DEVICE_CPSPEAKERS = 0x400,
            BASS_DEVICE_DEFAULT = 0,
            BASS_DEVICE_LATENCY = 0x100,
            BASS_DEVICE_MONO = 2,
            BASS_DEVICE_NOSPEAKER = 0x1000,
            BASS_DEVICE_SPEAKERS = 0x800
        }

        [Flags]
        public enum BASSInput
        {
            BASS_INPUT_NONE = 0,
            BASS_INPUT_OFF = 0x10000,
            BASS_INPUT_ON = 0x20000
        }

        [Flags]
        public enum BASSInputType
        {
            BASS_INPUT_TYPE_ANALOG = 0xa000000,
            BASS_INPUT_TYPE_AUX = 0x9000000,
            BASS_INPUT_TYPE_CD = 0x5000000,
            BASS_INPUT_TYPE_DIGITAL = 0x1000000,
            BASS_INPUT_TYPE_ERROR = -1,
            BASS_INPUT_TYPE_LINE = 0x2000000,
            BASS_INPUT_TYPE_MASK = -16777216,
            BASS_INPUT_TYPE_MIC = 0x3000000,
            BASS_INPUT_TYPE_PHONE = 0x6000000,
            BASS_INPUT_TYPE_SPEAKER = 0x7000000,
            BASS_INPUT_TYPE_SYNTH = 0x4000000,
            BASS_INPUT_TYPE_UNDEF = 0,
            BASS_INPUT_TYPE_WAVE = 0x8000000
        }

        [Flags]
        public enum BASSMode
        {
            BASS_MIDI_DECAYSEEK = 0x4000,
            BASS_MIXER_NORAMPIN = 0x800000,
            BASS_MUSIC_POSRESET = 0x8000,
            BASS_MUSIC_POSRESETEX = 0x400000,
            BASS_POS_BYTES = 0,
            BASS_POS_MIDI_TICK = 2,
            BASS_POS_MUSIC_ORDERS = 1
        }

        [Flags]
        public enum BASSRecordFormat
        {
            WAVE_FORMAT_1M08 = 1,
            WAVE_FORMAT_1M16 = 4,
            WAVE_FORMAT_1S08 = 2,
            WAVE_FORMAT_1S16 = 8,
            WAVE_FORMAT_2M08 = 0x10,
            WAVE_FORMAT_2M16 = 0x40,
            WAVE_FORMAT_2S08 = 0x20,
            WAVE_FORMAT_2S16 = 0x80,
            WAVE_FORMAT_48M08 = 0x1000,
            WAVE_FORMAT_48M16 = 0x4000,
            WAVE_FORMAT_48S08 = 0x2000,
            WAVE_FORMAT_48S16 = 0x8000,
            WAVE_FORMAT_4M08 = 0x100,
            WAVE_FORMAT_4M16 = 0x400,
            WAVE_FORMAT_4S08 = 0x200,
            WAVE_FORMAT_4S16 = 0x800,
            WAVE_FORMAT_96M08 = 0x10000,
            WAVE_FORMAT_96M16 = 0x40000,
            WAVE_FORMAT_96S08 = 0x20000,
            WAVE_FORMAT_96S16 = 0x80000,
            WAVE_FORMAT_UNKNOWN = 0
        }

        [Flags]
        public enum BASSRecordInfo
        {
            DSCAPS_CERTIFIED = 0x40,
            DSCAPS_EMULDRIVER = 0x20,
            DSCAPS_NONE = 0
        }

        public enum BASSStreamFilePosition
        {
            BASS_FILEPOS_BUFFER = 5,
            BASS_FILEPOS_CONNECTED = 4,
            BASS_FILEPOS_CURRENT = 0,
            BASS_FILEPOS_DOWNLOAD = 1,
            BASS_FILEPOS_END = 2,
            BASS_FILEPOS_START = 3,
            BASS_FILEPOS_WMA_BUFFER = 0x3e8
        }

        public enum BASSStreamProc
        {
            BASS_STREAMPROC_END = -2147483648,
            STREAMPROC_DUMMY = 0,
            STREAMPROC_PUSH = -1
        }

        public enum BASSStreamSystem
        {
            STREAMFILE_NOBUFFER,
            STREAMFILE_BUFFER,
            STREAMFILE_BUFFERPUSH
        }

        [Flags]
        public enum BASSSync
        {
            BASS_SYNC_CD_ERROR = 0x3e8,
            BASS_SYNC_CD_SPEED = 0x3ea,
            BASS_SYNC_DOWNLOAD = 7,
            BASS_SYNC_END = 2,
            BASS_SYNC_FREE = 8,
            BASS_SYNC_META = 4,
            BASS_SYNC_MIDI_CUE = 0x10001,
            BASS_SYNC_MIDI_EVENT = 0x10004,
            BASS_SYNC_MIDI_LYRIC = 0x10002,
            BASS_SYNC_MIDI_MARKER = 0x10000,
            BASS_SYNC_MIDI_TEXT = 0x10003,
            BASS_SYNC_MIDI_TICK = 0x10005,
            BASS_SYNC_MIDI_TIMESIG = 0x10006,
            BASS_SYNC_MIXER_ENVELOPE = 0x10200,
            BASS_SYNC_MIXTIME = 0x40000000,
            BASS_SYNC_MUSICFX = 3,
            BASS_SYNC_MUSICINST = 1,
            BASS_SYNC_MUSICPOS = 10,
            BASS_SYNC_OGG_CHANGE = 12,
            BASS_SYNC_ONETIME = -2147483648,
            BASS_SYNC_POS = 0,
            BASS_SYNC_SETPOS = 11,
            BASS_SYNC_SLIDE = 5,
            BASS_SYNC_STALL = 6,
            BASS_SYNC_WMA_CHANGE = 0x10100,
            BASS_SYNC_WMA_META = 0x10101,
            BASS_WINAMP_SYNC_BITRATE = 100
        }

        public enum BASSTag
        {
            BASS_TAG_ADX_LOOP = 0x12000,
            BASS_TAG_APE = 6,
            BASS_TAG_HTTP = 3,
            BASS_TAG_ICY = 4,
            BASS_TAG_ID3 = 0,
            BASS_TAG_ID3V2 = 1,
            BASS_TAG_LYRICS3 = 10,
            BASS_TAG_META = 5,
            BASS_TAG_MIDI_TRACK = 0x11000,
            BASS_TAG_MP4 = 7,
            BASS_TAG_MUSIC_INST = 0x10100,
            BASS_TAG_MUSIC_MESSAGE = 0x10001,
            BASS_TAG_MUSIC_NAME = 0x10000,
            BASS_TAG_MUSIC_ORDERS = 0x10002,
            BASS_TAG_MUSIC_SAMPLE = 0x10300,
            BASS_TAG_OGG = 2,
            BASS_TAG_RIFF_BEXT = 0x101,
            BASS_TAG_RIFF_CART = 0x102,
            BASS_TAG_RIFF_INFO = 0x100,
            BASS_TAG_UNKNOWN = -1,
            BASS_TAG_VENDOR = 9,
            BASS_TAG_WMA = 8,
            BASS_TAG_WMA_META = 11
        }

        [Flags]
        public enum BASSVam
        {
            BASS_VAM_HARDWARE = 1,
            BASS_VAM_SOFTWARE = 2,
            BASS_VAM_TERM_DIST = 8,
            BASS_VAM_TERM_PRIO = 0x10,
            BASS_VAM_TERM_TIME = 4
        }

        public enum EAXEnvironment
        {
            EAX_ENVIRONMENT_ALLEY = 14,
            EAX_ENVIRONMENT_ARENA = 9,
            EAX_ENVIRONMENT_AUDITORIUM = 6,
            EAX_ENVIRONMENT_BATHROOM = 3,
            EAX_ENVIRONMENT_CARPETEDHALLWAY = 11,
            EAX_ENVIRONMENT_CAVE = 8,
            EAX_ENVIRONMENT_CITY = 0x10,
            EAX_ENVIRONMENT_CONCERTHALL = 7,
            EAX_ENVIRONMENT_COUNT = 0x1a,
            EAX_ENVIRONMENT_DIZZY = 0x18,
            EAX_ENVIRONMENT_DRUGGED = 0x17,
            EAX_ENVIRONMENT_FOREST = 15,
            EAX_ENVIRONMENT_GENERIC = 0,
            EAX_ENVIRONMENT_HALLWAY = 12,
            EAX_ENVIRONMENT_HANGAR = 10,
            EAX_ENVIRONMENT_LEAVECURRENT = -1,
            EAX_ENVIRONMENT_LIVINGROOM = 4,
            EAX_ENVIRONMENT_MOUNTAINS = 0x11,
            EAX_ENVIRONMENT_PADDEDCELL = 1,
            EAX_ENVIRONMENT_PARKINGLOT = 20,
            EAX_ENVIRONMENT_PLAIN = 0x13,
            EAX_ENVIRONMENT_PSYCHOTIC = 0x19,
            EAX_ENVIRONMENT_QUARRY = 0x12,
            EAX_ENVIRONMENT_ROOM = 2,
            EAX_ENVIRONMENT_SEWERPIPE = 0x15,
            EAX_ENVIRONMENT_STONECORRIDOR = 13,
            EAX_ENVIRONMENT_STONEROOM = 5,
            EAX_ENVIRONMENT_UNDERWATER = 0x16
        }

        public enum EAXPreset
        {
            EAX_PRESET_GENERIC,
            EAX_PRESET_PADDEDCELL,
            EAX_PRESET_ROOM,
            EAX_PRESET_BATHROOM,
            EAX_PRESET_LIVINGROOM,
            EAX_PRESET_STONEROOM,
            EAX_PRESET_AUDITORIUM,
            EAX_PRESET_CONCERTHALL,
            EAX_PRESET_CAVE,
            EAX_PRESET_ARENA,
            EAX_PRESET_HANGAR,
            EAX_PRESET_CARPETEDHALLWAY,
            EAX_PRESET_HALLWAY,
            EAX_PRESET_STONECORRIDOR,
            EAX_PRESET_ALLEY,
            EAX_PRESET_FOREST,
            EAX_PRESET_CITY,
            EAX_PRESET_MOUNTAINS,
            EAX_PRESET_QUARRY,
            EAX_PRESET_PLAIN,
            EAX_PRESET_PARKINGLOT,
            EAX_PRESET_SEWERPIPE,
            EAX_PRESET_UNDERWATER,
            EAX_PRESET_DRUGGED,
            EAX_PRESET_DIZZY,
            EAX_PRESET_PSYCHOTIC
        }
        #endregion

        #region Delegate
        public delegate void DOWNLOADPROC(IntPtr buffer, int length, IntPtr user);

        public delegate void DSPPROC(int handle, int channel, IntPtr buffer, int length, IntPtr user);

        public delegate bool RECORDPROC(int handle, IntPtr buffer, int length, IntPtr user);

        public delegate int STREAMPROC(int handle, IntPtr buffer, int length, IntPtr user);

        public delegate void SYNCPROC(int handle, int channel, int data, IntPtr user);

        public delegate void FILECLOSEPROC(IntPtr user);
        public delegate long FILELENPROC(IntPtr user);
        public delegate int FILEREADPROC(IntPtr buffer, int length, IntPtr user);
        public delegate bool FILESEEKPROC(long offset, IntPtr user);
        #endregion

        #region Structures
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_3DVECTOR
        {
            public float x;
            public float y;
            public float z;

            public BASS_3DVECTOR()
            {
            }

            public BASS_3DVECTOR(float X, float Y, float Z)
            {
                this.x = X;
                this.y = Y;
                this.z = Z;
            }

            public override string ToString()
            {
                return string.Format("X={0}, Y={1}, Z={2}", this.x, this.y, this.z);
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_SAMPLE
        {
            public int freq;
            public float volume;
            public float pan;
            public BASSFlag flags;
            public int length;
            public int max;
            public int origres;
            public int chans;
            public int mingap;
            public BASS3DMode mode3d;
            public float mindist;
            public float maxdist;
            public int iangle;
            public int oangle;
            public float outvol;
            public BASSVam vam;
            public int priority;

            public BASS_SAMPLE()
            {
                this.freq = 0xac44;
                this.volume = 1f;
                this.max = 1;
                this.chans = 2;
                this.outvol = 1f;
                this.vam = BASSVam.BASS_VAM_HARDWARE;
            }

            public BASS_SAMPLE(int Freq, float Volume, float Pan, BASSFlag Flags, int Length, int Max, int OrigRes, int Chans, int MinGap, BASS3DMode Flag3D, float MinDist, float MaxDist, int IAngle, int OAngle, float OutVol, BASSVam FlagsVam, int Priority)
            {
                this.freq = 0xac44;
                this.volume = 1f;
                this.max = 1;
                this.chans = 2;
                this.outvol = 1f;
                this.vam = BASSVam.BASS_VAM_HARDWARE;
                this.freq = Freq;
                this.volume = Volume;
                this.pan = Pan;
                this.flags = Flags;
                this.length = Length;
                this.max = Max;
                this.origres = OrigRes;
                this.chans = Chans;
                this.mingap = MinGap;
                this.mode3d = Flag3D;
                this.mindist = MinDist;
                this.maxdist = MaxDist;
                this.iangle = IAngle;
                this.oangle = OAngle;
                this.outvol = OutVol;
                this.vam = FlagsVam;
                this.priority = Priority;
            }

            public override string ToString()
            {
                return string.Format("Frequency={0}, Volume={1}, Pan={2}", this.freq, this.volume, this.pan);
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_INFO
        {
            public BASSInfo flags;
            public int hwsize;
            public int hwfree;
            public int freesam;
            public int free3d;
            public int minrate;
            public int maxrate;
            public bool eax;
            public int minbuf = 500;
            public int dsver;
            public int latency;
            public BASSInit initflags;
            public int speakers;
            public int freq;

            public override string ToString()
            {
                return string.Format("Speakers={0}, MinRate={1}, MaxRate={2}, DX={3}, EAX={4}", new object[] { this.speakers, this.minrate, this.maxrate, this.dsver, this.eax });
            }

            public bool SupportsContinuousRate
            {
                get
                {
                    return ((this.flags & BASSInfo.DSCAPS_CONTINUOUSRATE) != BASSInfo.DSCAPS_NONE);
                }
            }
            public bool SupportsDirectSound
            {
                get
                {
                    return ((this.flags & BASSInfo.DSCAPS_EMULDRIVER) == BASSInfo.DSCAPS_NONE);
                }
            }
            public bool IsCertified
            {
                get
                {
                    return ((this.flags & BASSInfo.DSCAPS_CERTIFIED) != BASSInfo.DSCAPS_NONE);
                }
            }
            public bool SupportsMonoSamples
            {
                get
                {
                    return ((this.flags & BASSInfo.DSCAPS_SECONDARYMONO) != BASSInfo.DSCAPS_NONE);
                }
            }
            public bool SupportsStereoSamples
            {
                get
                {
                    return ((this.flags & BASSInfo.DSCAPS_SECONDARYSTEREO) != BASSInfo.DSCAPS_NONE);
                }
            }
            public bool Supports8BitSamples
            {
                get
                {
                    return ((this.flags & BASSInfo.DSCAPS_SECONDARY8BIT) != BASSInfo.DSCAPS_NONE);
                }
            }
            public bool Supports16BitSamples
            {
                get
                {
                    return ((this.flags & BASSInfo.DSCAPS_SECONDARY16BIT) != BASSInfo.DSCAPS_NONE);
                }
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_RECORDINFO
        {
            public BASSRecordInfo flags;
            public BASSRecordFormat formats;
            public int inputs;
            public bool singlein;
            public int freq;

            public override string ToString()
            {
                return string.Format("Inputs={0}, SingleIn={1}", this.inputs, this.singlein);
            }

            public bool SupportsDirectSound
            {
                get
                {
                    return ((this.flags & BASSRecordInfo.DSCAPS_EMULDRIVER) == BASSRecordInfo.DSCAPS_NONE);
                }
            }
            public bool IsCertified
            {
                get
                {
                    return ((this.flags & BASSRecordInfo.DSCAPS_CERTIFIED) != BASSRecordInfo.DSCAPS_NONE);
                }
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_CHANNELINFO
        {
            public int freq;
            public int chans;
            public BASSFlag flags;
            public BASSChannelType ctype;
            public int origres;
            public int plugin;
            public int sample;
            public IntPtr filename;

            public string FileName
            {
                get 
                {
                    if ((this.flags & (BASSFlag.BASS_DEFAULT | BASSFlag.BASS_UNICODE)) != BASSFlag.BASS_DEFAULT)
                    {
                        return Marshal.PtrToStringUni(this.filename);
                    }
                    if ((Environment.OSVersion.Platform == PlatformID.Unix) || (Environment.OSVersion.Platform == PlatformID.MacOSX))
                    {
                        if (this.filename != IntPtr.Zero)
                        {
                            int length = 0;

                            while (Marshal.ReadByte(this.filename, length) != 0)
                            {
                                length++;
                            }

                            if (length != 0)
                            {
                                byte[] destination = new byte[length];
                                Marshal.Copy(this.filename, destination, 0, length);
                                return Encoding.UTF8.GetString(destination, 0, length);
                            }
                        }

                        return null;
                    }

                    return Marshal.PtrToStringAnsi(this.filename);
                }
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_DEVICEINFO
        {
            public IntPtr name;
            public IntPtr driver;
            public BASSDeviceInfo flags;

            public string Name
            {
                get { return Marshal.PtrToStringAnsi(this.name); }
            }

            public string Driver
            {
                get { return Marshal.PtrToStringAnsi(this.driver); }
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public sealed class BASS_PLUGINFORM
        {
            public BASSChannelType ctype;
            [MarshalAs(UnmanagedType.LPStr)]
            public string name;
            [MarshalAs(UnmanagedType.LPStr)]
            public string exts;

            public BASS_PLUGINFORM()
            {
                this.name = string.Empty;
                this.exts = string.Empty;
            }

            public BASS_PLUGINFORM(string Name, string Extensions, BASSChannelType ChannelType)
            {
                this.name = string.Empty;
                this.exts = string.Empty;
                this.ctype = ChannelType;
                this.name = Name;
                this.exts = Extensions;
            }

            public override string ToString()
            {
                return string.Format("{0}|{1}", this.name, this.exts);
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_PLUGININFO
        {
            public int version;
            public int formatc;
            public IntPtr formats;
            //public BASS_PLUGINFORM[] formats;

        //    private BASS_PLUGININFO()
        //    {
        //    }

        //    public BASS_PLUGININFO(IntPtr pluginInfoPtr)
        //    {
        //        if (pluginInfoPtr != IntPtr.Zero)
        //        {
        //            c c = (c)Marshal.PtrToStructure(pluginInfoPtr, typeof(c));
        //            this.version = c.a;
        //            this.formatc = c.b;
        //            this.formats = new BASS_PLUGINFORM[this.formatc];
        //            this.a(this.formatc, c.c);
        //        }
        //    }

        //    internal BASS_PLUGININFO(int version, BASS_PLUGINFORM[] pluginForm)
        //    {
        //        this.version = version;
        //        this.formatc = pluginForm.Length;
        //        this.formats = pluginForm;
        //    }

        //    internal BASS_PLUGININFO(int version, int formatc, IntPtr handle)
        //    {
        //        this.version = version;
        //        this.formatc = formatc;
        //        if (handle != IntPtr.Zero)
        //        {
        //            this.formats = new BASS_PLUGINFORM[formatc];
        //            this.a(this.formatc, handle);
        //        }
        //    }

        //    private void a(int formatc, IntPtr handle)
        //    {
        //        for (int i = 0; i < formatc; i++)
        //        {
        //            this.formats[i] = (BASS_PLUGINFORM)Marshal.PtrToStructure(handle, typeof(BASS_PLUGINFORM));
        //            handle = new IntPtr(handle.ToPointer() + Marshal.SizeOf(this.formats[i]));
        //        }
        //    }

            public override string ToString()
            {
                return string.Format("{0}, {1}", this.version, this.formatc);
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_FILEPROCS
        {
            public FILECLOSEPROC close;
            public FILELENPROC length;
            public FILEREADPROC read;
            public FILESEEKPROC seek;

            public BASS_FILEPROCS(FILECLOSEPROC closeCallback, FILELENPROC lengthCallback, FILEREADPROC readCallback, FILESEEKPROC seekCallback)
            {
                this.close = closeCallback;
                this.length = lengthCallback;
                this.read = readCallback;
                this.seek = seekCallback;
            }
        }
        #endregion

        #region BASS_DX8 Structures
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_DX8_CHORUS
        {
            public float fWetDryMix;
            public float fDepth;
            public float fFeedback;
            public float fFrequency;
            public int lWaveform;
            public float fDelay;
            public BASSFXPhase lPhase;

            public BASS_DX8_CHORUS()
            {
                this.fDepth = 25f;
                this.lWaveform = 1;
                this.lPhase = BASSFXPhase.BASS_FX_PHASE_ZERO;
            }

            public BASS_DX8_CHORUS(float WetDryMix, float Depth, float Feedback, float Frequency, int Waveform, float Delay, BASSFXPhase Phase)
            {
                this.fDepth = 25f;
                this.lWaveform = 1;
                this.lPhase = BASSFXPhase.BASS_FX_PHASE_ZERO;
                this.fWetDryMix = WetDryMix;
                this.fDepth = Depth;
                this.fFeedback = Feedback;
                this.fFrequency = Frequency;
                this.lWaveform = Waveform;
                this.fDelay = Delay;
                this.lPhase = Phase;
            }

            public void Preset_Default()
            {
                this.fWetDryMix = 50f;
                this.fDepth = 25f;
                this.fFeedback = 0f;
                this.fFrequency = 0f;
                this.lWaveform = 1;
                this.fDelay = 0f;
                this.lPhase = BASSFXPhase.BASS_FX_PHASE_ZERO;
            }

            public void Preset_A()
            {
                this.fWetDryMix = 60f;
                this.fDepth = 60f;
                this.fFeedback = 25f;
                this.fFrequency = 5f;
                this.lWaveform = 1;
                this.fDelay = 8f;
                this.lPhase = BASSFXPhase.BASS_FX_PHASE_90;
            }

            public void Preset_B()
            {
                this.fWetDryMix = 75f;
                this.fDepth = 80f;
                this.fFeedback = 50f;
                this.fFrequency = 7f;
                this.lWaveform = 0;
                this.fDelay = 15f;
                this.lPhase = BASSFXPhase.BASS_FX_PHASE_NEG_90;
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_DX8_COMPRESSOR
        {
            public float fGain;
            public float fAttack;
            public float fRelease;
            public float fThreshold;
            public float fRatio;
            public float fPredelay;

            public BASS_DX8_COMPRESSOR()
            {
                this.fAttack = 10f;
                this.fRelease = 200f;
                this.fThreshold = -20f;
                this.fRatio = 3f;
                this.fPredelay = 4f;
            }

            public BASS_DX8_COMPRESSOR(float Gain, float Attack, float Release, float Threshold, float Ratio, float Predelay)
            {
                this.fAttack = 10f;
                this.fRelease = 200f;
                this.fThreshold = -20f;
                this.fRatio = 3f;
                this.fPredelay = 4f;
                this.fGain = Gain;
                this.fAttack = Attack;
                this.fRelease = Release;
                this.fThreshold = Threshold;
                this.fRatio = Ratio;
                this.fPredelay = Predelay;
            }

            public void Preset_Default()
            {
                this.fGain = 0f;
                this.fAttack = 10f;
                this.fRelease = 200f;
                this.fThreshold = -20f;
                this.fRatio = 3f;
                this.fPredelay = 4f;
            }

            public void Preset_Soft()
            {
                this.fGain = 0f;
                this.fAttack = 12f;
                this.fRelease = 800f;
                this.fThreshold = -20f;
                this.fRatio = 3f;
                this.fPredelay = 4f;
            }

            public void Preset_Soft2()
            {
                this.fGain = 2f;
                this.fAttack = 20f;
                this.fRelease = 800f;
                this.fThreshold = -20f;
                this.fRatio = 4f;
                this.fPredelay = 4f;
            }

            public void Preset_Medium()
            {
                this.fGain = 4f;
                this.fAttack = 5f;
                this.fRelease = 600f;
                this.fThreshold = -20f;
                this.fRatio = 5f;
                this.fPredelay = 3f;
            }

            public void Preset_Hard()
            {
                this.fGain = 2f;
                this.fAttack = 2f;
                this.fRelease = 400f;
                this.fThreshold = -20f;
                this.fRatio = 8f;
                this.fPredelay = 2f;
            }

            public void Preset_Hard2()
            {
                this.fGain = 6f;
                this.fAttack = 2f;
                this.fRelease = 200f;
                this.fThreshold = -20f;
                this.fRatio = 10f;
                this.fPredelay = 2f;
            }

            public void Preset_HardCommercial()
            {
                this.fGain = 4f;
                this.fAttack = 5f;
                this.fRelease = 300f;
                this.fThreshold = -16f;
                this.fRatio = 9f;
                this.fPredelay = 2f;
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_DX8_DISTORTION
        {
            public float fGain;
            public float fEdge;
            public float fPostEQCenterFrequency;
            public float fPostEQBandwidth;
            public float fPreLowpassCutoff;

            public BASS_DX8_DISTORTION()
            {
                this.fEdge = 50f;
                this.fPostEQCenterFrequency = 4000f;
                this.fPostEQBandwidth = 4000f;
                this.fPreLowpassCutoff = 4000f;
            }

            public BASS_DX8_DISTORTION(float Gain, float Edge, float PostEQCenterFrequency, float PostEQBandwidth, float PreLowpassCutoff)
            {
                this.fEdge = 50f;
                this.fPostEQCenterFrequency = 4000f;
                this.fPostEQBandwidth = 4000f;
                this.fPreLowpassCutoff = 4000f;
                this.fGain = Gain;
                this.fEdge = Edge;
                this.fPostEQCenterFrequency = PostEQCenterFrequency;
                this.fPostEQBandwidth = PostEQBandwidth;
                this.fPreLowpassCutoff = PreLowpassCutoff;
            }

            public void Preset_Default()
            {
                this.fGain = 0f;
                this.fEdge = 50f;
                this.fPostEQCenterFrequency = 4000f;
                this.fPostEQBandwidth = 4000f;
                this.fPreLowpassCutoff = 4000f;
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_DX8_ECHO
        {
            public float fWetDryMix;
            public float fFeedback;
            public float fLeftDelay;
            public float fRightDelay;
            [MarshalAs(UnmanagedType.Bool)]
            public bool lPanDelay;

            public BASS_DX8_ECHO()
            {
                this.fLeftDelay = 333f;
                this.fRightDelay = 333f;
            }

            public BASS_DX8_ECHO(float WetDryMix, float Feedback, float LeftDelay, float RightDelay, bool PanDelay)
            {
                this.fLeftDelay = 333f;
                this.fRightDelay = 333f;
                this.fWetDryMix = WetDryMix;
                this.fFeedback = Feedback;
                this.fLeftDelay = LeftDelay;
                this.fRightDelay = RightDelay;
                this.lPanDelay = PanDelay;
            }

            public void Preset_Default()
            {
                this.fWetDryMix = 50f;
                this.fFeedback = 0f;
                this.fLeftDelay = 333f;
                this.fRightDelay = 333f;
                this.lPanDelay = false;
            }

            public void Preset_Small()
            {
                this.fWetDryMix = 50f;
                this.fFeedback = 20f;
                this.fLeftDelay = 100f;
                this.fRightDelay = 100f;
                this.lPanDelay = false;
            }

            public void Preset_Long()
            {
                this.fWetDryMix = 50f;
                this.fFeedback = 20f;
                this.fLeftDelay = 700f;
                this.fRightDelay = 700f;
                this.lPanDelay = false;
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_DX8_FLANGER
        {
            public float fWetDryMix;
            public float fDepth;
            public float fFeedback;
            public float fFrequency;
            public int lWaveform;
            public float fDelay;
            public BASSFXPhase lPhase;

            public BASS_DX8_FLANGER()
            {
                this.fDepth = 25f;
                this.lWaveform = 1;
                this.lPhase = BASSFXPhase.BASS_FX_PHASE_ZERO;
            }

            public BASS_DX8_FLANGER(float WetDryMix, float Depth, float Feedback, float Frequency, int Waveform, float Delay, BASSFXPhase Phase)
            {
                this.fDepth = 25f;
                this.lWaveform = 1;
                this.lPhase = BASSFXPhase.BASS_FX_PHASE_ZERO;
                this.fWetDryMix = WetDryMix;
                this.fDepth = Depth;
                this.fFeedback = Feedback;
                this.fFrequency = Frequency;
                this.lWaveform = Waveform;
                this.fDelay = Delay;
                this.lPhase = Phase;
            }

            public void Preset_Default()
            {
                this.fWetDryMix = 50f;
                this.fDepth = 25f;
                this.fFeedback = 0f;
                this.fFrequency = 0f;
                this.lWaveform = 1;
                this.fDelay = 0f;
                this.lPhase = BASSFXPhase.BASS_FX_PHASE_ZERO;
            }

            public void Preset_A()
            {
                this.fWetDryMix = 60f;
                this.fDepth = 60f;
                this.fFeedback = 25f;
                this.fFrequency = 5f;
                this.lWaveform = 1;
                this.fDelay = 1f;
                this.lPhase = BASSFXPhase.BASS_FX_PHASE_90;
            }

            public void Preset_B()
            {
                this.fWetDryMix = 75f;
                this.fDepth = 80f;
                this.fFeedback = 50f;
                this.fFrequency = 7f;
                this.lWaveform = 0;
                this.fDelay = 3f;
                this.lPhase = BASSFXPhase.BASS_FX_PHASE_NEG_90;
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_DX8_GARGLE
        {
            public int dwRateHz;
            public int dwWaveShape;

            public BASS_DX8_GARGLE()
            {
                this.dwRateHz = 500;
                this.dwWaveShape = 1;
            }

            public BASS_DX8_GARGLE(int RateHz, int WaveShape)
            {
                this.dwRateHz = 500;
                this.dwWaveShape = 1;
                this.dwRateHz = RateHz;
                this.dwWaveShape = WaveShape;
            }

            public void Preset_Default()
            {
                this.dwRateHz = 100;
                this.dwWaveShape = 1;
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_DX8_I3DL2REVERB
        {
            public int lRoom;
            public int lRoomHF;
            public float flRoomRolloffFactor;
            public float flDecayTime;
            public float flDecayHFRatio;
            public int lReflections;
            public float flReflectionsDelay;
            public int lReverb;
            public float flReverbDelay;
            public float flDiffusion;
            public float flDensity;
            public float flHFReference;

            public BASS_DX8_I3DL2REVERB()
            {
                this.lRoom = -1000;
                this.flDecayTime = 1.49f;
                this.flDecayHFRatio = 0.83f;
                this.lReflections = -2602;
                this.flReflectionsDelay = 0.007f;
                this.lReverb = 200;
                this.flReverbDelay = 0.011f;
                this.flDiffusion = 100f;
                this.flDensity = 100f;
                this.flHFReference = 5000f;
            }

            public BASS_DX8_I3DL2REVERB(int Room, int RoomHF, float RoomRolloffFactor, float DecayTime, float DecayHFRatio, int Reflections, float ReflectionsDelay, int Reverb, float ReverbDelay, float Diffusion, float Density, float HFReference)
            {
                this.lRoom = -1000;
                this.flDecayTime = 1.49f;
                this.flDecayHFRatio = 0.83f;
                this.lReflections = -2602;
                this.flReflectionsDelay = 0.007f;
                this.lReverb = 200;
                this.flReverbDelay = 0.011f;
                this.flDiffusion = 100f;
                this.flDensity = 100f;
                this.flHFReference = 5000f;
                this.lRoom = Room;
                this.lRoomHF = RoomHF;
                this.flRoomRolloffFactor = RoomRolloffFactor;
                this.flDecayTime = DecayTime;
                this.flDecayHFRatio = DecayHFRatio;
                this.lReflections = Reflections;
                this.flReflectionsDelay = ReflectionsDelay;
                this.lReverb = Reverb;
                this.flReverbDelay = ReverbDelay;
                this.flDiffusion = Diffusion;
                this.flDensity = Density;
                this.flHFReference = HFReference;
            }

            public void Preset_Default()
            {
                this.lRoom = -1000;
                this.lRoomHF = 0;
                this.flRoomRolloffFactor = 0f;
                this.flDecayTime = 1.49f;
                this.flDecayHFRatio = 0.83f;
                this.lReflections = -2602;
                this.flReflectionsDelay = 0.007f;
                this.lReverb = 200;
                this.flReverbDelay = 0.011f;
                this.flDiffusion = 100f;
                this.flDensity = 100f;
                this.flHFReference = 5000f;
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_DX8_PARAMEQ
        {
            public float fCenter;
            public float fBandwidth;
            public float fGain;

            public BASS_DX8_PARAMEQ()
            {
                this.fCenter = 100f;
                this.fBandwidth = 18f;
            }

            public BASS_DX8_PARAMEQ(float Center, float Bandwidth, float Gain)
            {
                this.fCenter = 100f;
                this.fBandwidth = 18f;
                this.fCenter = Center;
                this.fBandwidth = Bandwidth;
                this.fGain = Gain;
            }

            public void Preset_Default()
            {
                this.fCenter = 100f;
                this.fBandwidth = 18f;
                this.fGain = 0f;
            }

            public void Preset_Low()
            {
                this.fCenter = 125f;
                this.fBandwidth = 18f;
                this.fGain = 0f;
            }

            public void Preset_Mid()
            {
                this.fCenter = 1000f;
                this.fBandwidth = 18f;
                this.fGain = 0f;
            }

            public void Preset_High()
            {
                this.fCenter = 8000f;
                this.fBandwidth = 18f;
                this.fGain = 0f;
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class BASS_DX8_REVERB
        {
            public float fInGain;
            public float fReverbMix;
            public float fReverbTime;
            public float fHighFreqRTRatio;

            public BASS_DX8_REVERB()
            {
                this.fReverbTime = 1000f;
                this.fHighFreqRTRatio = 0.001f;
            }

            public BASS_DX8_REVERB(float InGain, float ReverbMix, float ReverbTime, float HighFreqRTRatio)
            {
                this.fReverbTime = 1000f;
                this.fHighFreqRTRatio = 0.001f;
                this.fInGain = InGain;
                this.fReverbMix = ReverbMix;
                this.fReverbTime = ReverbTime;
                this.fHighFreqRTRatio = HighFreqRTRatio;
            }

            public void Preset_Default()
            {
                this.fInGain = -3f;
                this.fReverbMix = -6f;
                this.fReverbTime = 1000f;
                this.fHighFreqRTRatio = 0.5f;
            }
        }
        #endregion
    }
}
