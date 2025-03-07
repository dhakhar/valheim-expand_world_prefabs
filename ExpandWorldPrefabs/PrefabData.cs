using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Data;
using Service;
using UnityEngine;

namespace ExpandWorld.Prefab;

public class Data
{
  [DefaultValue("")]
  public string prefab = "";
  public string type = "";
  [DefaultValue(null)]
  public string[]? types = null;
  [DefaultValue(false)]
  public bool fallback = false;
  [DefaultValue(1f)]
  public float weight = 1f;
  [DefaultValue(null)]
  public string? swap = null;
  [DefaultValue(null)]
  public string[]? swaps = null;
  [DefaultValue(null)]
  public string? spawn = null;
  [DefaultValue(null)]
  public string[]? spawns = null;
  [DefaultValue(0f)]
  public float spawnDelay = 0f;
  [DefaultValue(false)]
  public bool remove = false;
  [DefaultValue(0f)]
  public float removeDelay = 0f;
  [DefaultValue(false)]
  public bool drops = false;
  [DefaultValue("")]
  public string data = "";
  [DefaultValue(null)]
  public string? command = null;
  [DefaultValue(null)]
  public string[]? commands = null;
  [DefaultValue(true)]
  public bool day = true;
  [DefaultValue(true)]
  public bool night = true;
  [DefaultValue("")]
  public string biomes = "";
  [DefaultValue(0f)]
  public float minDistance = 0f;
  [DefaultValue(100000f)]
  public float maxDistance = 100000f;
  [DefaultValue(-10000f)]
  public float minAltitude = -10000f;
  [DefaultValue(10000f)]
  public float maxAltitude = 10000f;
  [DefaultValue(null)]
  public float? minY = null;
  [DefaultValue(null)]
  public float? maxY = null;
  [DefaultValue("")]
  public string environments = "";
  [DefaultValue("")]
  public string bannedEnvironments = "";
  [DefaultValue("")]
  public string globalKeys = "";
  [DefaultValue("")]
  public string bannedGlobalKeys = "";
  [DefaultValue("")]
  public string events = "";
  [DefaultValue(null)]
  public float? eventDistance = null;
  [DefaultValue(null)]
  public PokeData[]? poke = null;
  [DefaultValue(null)]
  public string[]? pokes = null;
  [DefaultValue(0)]
  public int pokeLimit = 0;
  [DefaultValue("")]
  public string pokeParameter = "";
  [DefaultValue(0f)]
  public float pokeDelay = 0f;

  [DefaultValue(null)]
  public string[]? objects = null;
  [DefaultValue("")]
  public string objectsLimit = "";
  [DefaultValue(null)]
  public string[]? bannedObjects = null;
  [DefaultValue("")]
  public string bannedObjectsLimit = "";
  [DefaultValue("")]
  public string locations = "";
  [DefaultValue(0f)]
  public float locationDistance = 0f;
  [DefaultValue(null)]
  public string? filter = null;
  [DefaultValue(null)]
  public string[]? filters = null;
  [DefaultValue(null)]
  public string? bannedFilter = null;
  [DefaultValue(null)]
  public string[]? bannedFilters = null;
  [DefaultValue(0f)]
  public float delay = 0f;

  [DefaultValue(false)]
  public bool triggerRules = false;
  [DefaultValue(null)]
  public Dictionary<string, string>[]? objectRpc = null;
  [DefaultValue(null)]
  public Dictionary<string, string>[]? clientRpc = null;

  [DefaultValue("")]
  public string minPaint = "";
  [DefaultValue("")]
  public string maxPaint = "";
  [DefaultValue("")]
  public string paint = "";

  [DefaultValue(false)]
  public bool injectData = false;
}


public class Info
{
  public string Prefabs = "";
  public ActionType Type = ActionType.Create;
  public bool Fallback = false;
  public string[] Args = [];
  public float Weight = 1f;
  public Spawn[] Swaps = [];
  public Spawn[] Spawns = [];
  public bool Remove = false;
  public float RemoveDelay = 0f;
  public bool Drops = false;
  public string Data = "";
  public bool InjectData = false;
  public string[] Commands = [];
  public bool Day = true;
  public bool Night = true;
  public float MinDistance = 0f;
  public float MaxDistance = 0f;
  public float MinY = 0f;
  public float MaxY = 0f;
  public Heightmap.Biome Biomes = Heightmap.Biome.None;
  public float EventDistance = 0f;
  public HashSet<string> Events = [];
  public HashSet<string> Environments = [];
  public HashSet<string> BannedEnvironments = [];
  public List<string> GlobalKeys = [];
  public List<string> BannedGlobalKeys = [];
  public Object[] LegacyPokes = [];
  public Poke[] Pokes = [];
  public int PokeLimit = 0;
  public string PokeParameter = "";
  public float PokeDelay = 0f;
  public Range<int>? ObjectsLimit = null;
  public Object[] Objects = [];
  public Range<int>? BannedObjectsLimit = null;
  public Object[] BannedObjects = [];
  public HashSet<string> Locations = [];
  public float LocationDistance = 0f;
  public DataEntry? Filter;
  public DataEntry? BannedFilter;
  public bool TriggerRules = false;
  public ObjectRpcInfo[]? ObjectRpcs;
  public ClientRpcInfo[]? ClientRpcs;
  public Color? MinPaint;
  public Color? MaxPaint;
}
public class Spawn
{
  private readonly int Prefab = 0;
  private readonly string WildPrefab = "";
  public readonly Vector3 Pos = Vector3.zero;
  public readonly bool Snap = false;
  public readonly Quaternion Rot = Quaternion.identity;
  public readonly string Data = "";
  public readonly float Delay = 0;
  public Spawn(string line, float delay)
  {
    Delay = delay;
    var split = Parse.ToList(line);
    if (split[0].Contains("<") && split[0].Contains(">"))
      WildPrefab = split[0];
    else
    {
      Prefab = split[0].GetStableHashCode();
      Prefab = ZNetScene.instance.GetPrefab(Prefab) ? Prefab : 0;
    }
    if (split.Count == 2)
    {
      Data = split[1];
    }
    if (split.Count > 3)
    {
      if (Parse.TryFloat(split[1], out var x))
        Pos = new Vector3(x, Parse.Float(split[3]), Parse.Float(split[2]));
      if (split[3] == "snap")
        Snap = true;
    }
    if (split.Count > 6)
    {
      if (Parse.TryFloat(split[4], out var x))
        Rot = Quaternion.Euler(Parse.Float(split[5]), x, Parse.Float(split[6]));
      else
        Data = split[4];
    }
    if (split.Count > 7)
      Data = split[7];
    if (split.Count > 8)
      Delay = Parse.Float(split[8]);
  }
  public int GetPrefab(Pars pars)
  {
    if (Prefab != 0) return Prefab;
    var prefabName = Helper.ReplaceParameters(WildPrefab, pars, null);
    var prefab = prefabName.GetStableHashCode();
    return ZNetScene.instance.GetPrefab(prefab) ? prefab : 0;
  }
}

public class Poke(PokeData data)
{
  public Object Filter = new(data.prefab, data.minDistance, data.maxDistance, data.data);
  public string Parameter = data.parameter;
  public int Limit = data.limit;
  public float Delay = data.delay;
}
public class Object
{
  public int Prefab = 0;
  public HashSet<int>? Prefabs;
  public string WildPrefab = "";
  public float MinDistance = 0f;
  public float MaxDistance = 100f;
  public int Data = 0;
  public int Weight = 1;
  public Object(string prefab, float minDistance, float maxDistance, string data)
  {
    ParsePrefabs(prefab);
    MinDistance = minDistance;
    if (maxDistance > 0)
      MaxDistance = maxDistance;
    if (data != "")
    {
      Data = data.GetStableHashCode();
      if (!DataHelper.Exists(Data))
      {
        Log.Error($"Invalid object filter data: {data}");
        Data = 0;
      }
    }
  }
  public Object(string line)
  {
    var split = Parse.ToList(line);
    ParsePrefabs(split[0]);

    if (split.Count > 1)
    {
      var range = Parse.FloatRange(split[1]);
      MinDistance = range.Min == range.Max ? 0f : range.Min;
      MaxDistance = range.Max;
    }
    if (split.Count > 2)
    {
      Data = split[2].GetStableHashCode();
      if (!DataHelper.Exists(Data))
      {
        Log.Error($"Invalid object filter data: {split[2]}");
        Data = 0;
      }
    }
    if (split.Count > 3)
    {
      Weight = Parse.Int(split[3]);
    }
  }
  private void ParsePrefabs(string prefabs)
  {
    if (prefabs.Contains("<") && prefabs.Contains(">"))
      WildPrefab = prefabs;
    else
    {
      var hash = prefabs.GetStableHashCode();
      if (ZNetScene.instance.m_namedPrefabs.ContainsKey(hash))
        Prefab = hash;
      else
      {
        var split = Parse.ToList(prefabs);
        Prefabs = [];
        foreach (var prefab in split)
        {
          hash = prefab.GetStableHashCode();
          if (ZNetScene.instance.m_namedPrefabs.ContainsKey(hash))
            Prefabs.Add(hash);
          else
          {
            var values = DataHelper.GetValuesFromGroup(prefab);
            if (values == null)
              Log.Error($"Invalid object filter prefab: {prefab}");
            else
              Prefabs.UnionWith(values.Select(v => v.GetStableHashCode()));
          }
        }
      }
    }
  }
  public bool IsValid(ZDO zdo, Vector3 pos, Dictionary<string, string> parameters)
  {
    if (Prefab != 0 && zdo.GetPrefab() != Prefab) return false;
    if (Prefabs != null && !Prefabs.Contains(zdo.GetPrefab())) return false;
    if (WildPrefab != "")
    {
      var prefabName = Helper.ReplaceParameters(WildPrefab, parameters, zdo);
      var hash = prefabName.GetStableHashCode();
      if (zdo.GetPrefab() != hash) return false;
    }
    var d = Utils.DistanceXZ(pos, zdo.GetPosition());
    if (MinDistance > 0f && d < MinDistance) return false;
    if (d > MaxDistance) return false;
    if (Data == 0) return true;
    return DataHelper.Match(Data, zdo, parameters);
  }
}

public class PokeData
{
  [DefaultValue("")]
  public string prefab = "";
  [DefaultValue(0f)]
  public float delay = 0f;
  [DefaultValue("")]
  public string parameter = "";
  [DefaultValue(0f)]
  public float maxDistance = 0f;
  [DefaultValue(0f)]
  public float minDistance = 0f;
  [DefaultValue(0)]
  public int limit = 0;
  [DefaultValue("")]
  public string data = "";
}

public class InfoType
{
  public readonly ActionType Type;
  public readonly string[] Parameters;
  public InfoType(string prefab, string line)
  {
    var types = Parse.ToList(line);
    if (types.Count == 0 || !Enum.TryParse(types[0], true, out Type))
    {
      if (line == "")
        Log.Warning($"Missing type for prefab {prefab}.");
      else
        Log.Error($"Failed to parse type {prefab}.");
      Type = ActionType.Create;
    }
    Parameters = types.Count > 1 ? types[1].Split(' ') : [];
  }
}