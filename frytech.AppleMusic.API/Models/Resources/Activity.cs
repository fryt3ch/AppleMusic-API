﻿using frytech.AppleMusic.API.Models.Attributes;
using frytech.AppleMusic.API.Models.Core;
using frytech.AppleMusic.API.Models.Enums;
using frytech.AppleMusic.API.Models.Relationships;

namespace frytech.AppleMusic.API.Models.Resources;

/// <summary>
/// A Resource object that represents an activity.
/// https://developer.apple.com/documentation/applemusicapi/activity
/// </summary>
/// <inheritdoc />
public class Activity : Resource, IHasAttributes<ActivityAttributes>, IHasRelationships<ActivityRelationships>
{
    public override ResourceType Type => ResourceType.Activities;
    
    public ActivityAttributes Attributes { get; set; }
    
    public ActivityRelationships Relationships { get; set; }
}