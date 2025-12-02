using Godot;
using SoulashSaveUtils.Types;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
    }

    public void UpdateSkills(FrozenDictionary<string, Skill> skills)
    {
        foreach (var child in GetChildren())
        {
            RemoveChild(child);
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
            AddChild(skillTag);
        }
    }
}
