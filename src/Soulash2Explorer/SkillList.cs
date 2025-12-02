/*
  Copyright (C) 2025, S2E Contributors

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Lesser General Public License as published 
  by the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.
*/

using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Godot;
using SoulashSaveUtils.Types;
namespace Soulash2Explorer;

public partial class SkillList : GridContainer
{
  [Export]
  public PackedScene SkillTagPackedScene;

  private IReadOnlyCollection<SkillTag> _skillTags;

  public void InitializeSkillTags(int count)
  {
    _skillTags = Enumerable.Range(0, count)
        .Select((i) => SkillTagPackedScene.Instantiate<SkillTag>())
        .ToImmutableArray();

    foreach (var tag in _skillTags)
    {
      tag.Visible = false;
      AddChild(tag);
    }
  }

  public void UpdateSkills(FrozenDictionary<string, Skill> skills)
  {
    foreach (var child in GetChildren())
    {
      if (child is SkillTag tag)
        tag.Visible = false;
    }

    if (skills == null || skills.Count == 0)
    {
      return;
    }

    foreach (var (skillTag, skill) in _skillTags
        .Take(skills.Count)
        .Select((skillTag, index) => (skillTag, skills.ElementAt(index).Value))
    )
    {
      skillTag.UpdateSkill(skill);
      skillTag.Visible = true;
    }
  }
}
