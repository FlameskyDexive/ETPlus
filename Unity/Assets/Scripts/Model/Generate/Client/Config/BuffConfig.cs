//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;


namespace ET
{

public sealed partial class BuffConfig: Bright.Config.BeanBase
{
    public BuffConfig(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        Name = _buf.ReadString();
        Desc = _buf.ReadString();
        BuffType = _buf.ReadInt();
        NumericType = _buf.ReadInt();
        NumericValue = _buf.ReadInt();
        Duration = _buf.ReadInt();
        Interval = _buf.ReadInt();
        AddType = _buf.ReadInt();
        MaxNum = _buf.ReadInt();
        ReduceType = _buf.ReadInt();
        {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);Tags = new System.Collections.Generic.List<int>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { int _e0;  _e0 = _buf.ReadInt(); Tags.Add(_e0);}}
        Goup = _buf.ReadInt();
        EffectRoot = _buf.ReadString();
        EffectRes = _buf.ReadString();
        EffectScale = _buf.ReadInt();
        TriggerEffectRoot = _buf.ReadString();
        TriggerEffectRes = _buf.ReadString();
        TriggerEffectScale = _buf.ReadInt();
        PostInit();
    }

    public static BuffConfig DeserializeBuffConfig(ByteBuf _buf)
    {
        return new BuffConfig(_buf);
    }

    /// <summary>
    /// buffID
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Desc { get; private set; }
    /// <summary>
    /// 效果类型
    /// </summary>
    public int BuffType { get; private set; }
    /// <summary>
    /// 效果参数
    /// </summary>
    public int NumericType { get; private set; }
    /// <summary>
    /// 效果值
    /// </summary>
    public int NumericValue { get; private set; }
    /// <summary>
    /// 持续时间
    /// </summary>
    public int Duration { get; private set; }
    /// <summary>
    /// 触发间隔
    /// </summary>
    public int Interval { get; private set; }
    /// <summary>
    /// 叠加方式
    /// </summary>
    public int AddType { get; private set; }
    /// <summary>
    /// 最大叠加层数
    /// </summary>
    public int MaxNum { get; private set; }
    /// <summary>
    /// 减少方式
    /// </summary>
    public int ReduceType { get; private set; }
    /// <summary>
    /// 标签
    /// </summary>
    public System.Collections.Generic.List<int> Tags { get; private set; }
    /// <summary>
    /// 分组
    /// </summary>
    public int Goup { get; private set; }
    /// <summary>
    /// 特效挂点
    /// </summary>
    public string EffectRoot { get; private set; }
    /// <summary>
    /// 特效
    /// </summary>
    public string EffectRes { get; private set; }
    /// <summary>
    /// 特效缩放(100倍)
    /// </summary>
    public int EffectScale { get; private set; }
    /// <summary>
    /// 触发时的特效挂点
    /// </summary>
    public string TriggerEffectRoot { get; private set; }
    /// <summary>
    /// 触发时的特效
    /// </summary>
    public string TriggerEffectRes { get; private set; }
    /// <summary>
    /// 触发时的特效缩放
    /// </summary>
    public int TriggerEffectScale { get; private set; }

    public const int __ID__ = -1370631787;
    public override int GetTypeId() => __ID__;

    public  void Resolve(ConcurrentDictionary<Type, IConfigSingleton> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "Name:" + Name + ","
        + "Desc:" + Desc + ","
        + "BuffType:" + BuffType + ","
        + "NumericType:" + NumericType + ","
        + "NumericValue:" + NumericValue + ","
        + "Duration:" + Duration + ","
        + "Interval:" + Interval + ","
        + "AddType:" + AddType + ","
        + "MaxNum:" + MaxNum + ","
        + "ReduceType:" + ReduceType + ","
        + "Tags:" + Bright.Common.StringUtil.CollectionToString(Tags) + ","
        + "Goup:" + Goup + ","
        + "EffectRoot:" + EffectRoot + ","
        + "EffectRes:" + EffectRes + ","
        + "EffectScale:" + EffectScale + ","
        + "TriggerEffectRoot:" + TriggerEffectRoot + ","
        + "TriggerEffectRes:" + TriggerEffectRes + ","
        + "TriggerEffectScale:" + TriggerEffectScale + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}