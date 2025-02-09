﻿using frytech.AppleMusic.API.Models.Core;
using frytech.AppleMusic.API.Models.Requests;

namespace frytech.AppleMusic.API.Models.Relationships;

/// <summary>
/// The relationships for a library playlist creation request object.
/// https://developer.apple.com/documentation/applemusicapi/libraryplaylistcreationrequest/relationships
/// </summary>
public class LibraryPlaylistCreationRequestRelationships : IRelationships
{
    /// <summary>
    /// The songs and music videos added to this playlist when it is created.
    /// </summary>
    public List<LibraryPlaylistRequestTrack> Tracks { get; set; }
}