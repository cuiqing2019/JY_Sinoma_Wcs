using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace Sinoma_WCS
{

    public class Speech
    {
        //#region 全局变量
        ///// <summary>
        ///// 语音合成对象
        ///// </summary>
        //private SpeechSynthesizer synth;
        ///// <summary>
        ///// 语音参数结构体对象
        ///// </summary>
        //private SpeechStruct speechStruct = new SpeechStruct();
        ///// <summary>
        ///// 语音参数结构体
        ///// </summary>
        //private struct SpeechStruct
        //{
        //    public int voiceRate;     //语速
        //    public int voiceVolume;   //音量
        //    public bool voice_inited; //是否初始化完成
        //}
        //#endregion 

        //#region 构造函数
        ///// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="voiceRate">语速</param>
        ///// <param name="voiceVolume">音量</param>
        //public Speech(int voiceRate,int voiceVolume)
        //{            
        //    this.speechStruct.voiceRate = voiceRate;
        //    this.speechStruct.voiceVolume = voiceVolume;
        //    this.speechStruct.voice_inited = false;
        //    init_voice();
        //}
        //#endregion 

        //#region 语音对象初始化
        ///// <summary>
        ///// 语音对象初始化
        ///// </summary>
        //public bool init_voice()
        //{
        //    try
        //    {
        //        synth = new SpeechSynthesizer();
                
        //        synth.Rate = speechStruct.voiceRate;
        //        synth.Volume = speechStruct.voiceVolume;
        //        speechStruct.voice_inited = true;
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        speechStruct.voice_inited = false;
        //        MessageBox.Show(ex.ToString(), "语音库加载失败", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        //    }
        //    return false;
        //}
        //#endregion 

        //#region 语音播报
        ///// <summary>
        ///// 语音播报
        ///// </summary>
        ///// <param name="txt">播放内容</param>
        //public void speech(string txt)
        //{
        //    try
        //    {
        //        if (speechStruct.voice_inited)
        //        {
        //            PromptBuilder builder = new PromptBuilder();
        //            builder.AppendText(txt);
        //            synth.SpeakAsync(builder);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.ToString());
        //    }
        //}
        //#endregion
    }
}
