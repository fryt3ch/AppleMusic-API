﻿using System.Text.Json.Serialization;
using frytech.AppleMusic.API.Models.Enums;

namespace frytech.AppleMusic.API.Models.Attributes;

/// <summary>
/// The attributes for a song object.
/// https://developer.apple.com/documentation/applemusicapi/song/attributes
/// </summary>
public class SongAttributes : ITrackAttributes
{
    /// <summary>
    /// (Required) The name of the album the song appears on.
    /// </summary>
    public string AlbumName { get; set; }

    /// <summary>
    /// (Required) The artist’s name.
    /// </summary>
    public string ArtistName { get; set; }

    /// <summary>
    /// (Required) The album artwork.
    /// </summary>
    public Artwork? Artwork { get; set; }

    /// <summary>
    /// The song’s composer.
    /// </summary>
    public string ComposerName { get; set; }

    /// <summary>
    /// The Recording Industry Association of America (RIAA) rating of the content. The possible values for this rating are clean and explicit. No value means no rating.
    /// </summary>
    public ContentRating ContentRating { get; set; }

    /// <summary>
    /// (Required) The disc number the song appears on.
    /// </summary>
    public int DiskNumber { get; set; }

    /// <summary>
    /// The duration of the song in milliseconds.
    /// </summary>
    [JsonPropertyName("durationInMillis")]
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// The notes about the song that appear in the iTunes Store.
    /// </summary>
    public EditorialNotes EditorialNotes { get; set; }

    /// <summary>
    /// (Required) The genre names the song is associated with.
    /// </summary>
    public List<string> GenreNames { get; set; }

    /// <summary>
    /// (Required) The International Standard Recording Code (ISRC) for the song.
    /// </summary>
    public string Isrc { get; set; }

    /// <summary>
    /// (Classical music only) The movement count of this song.
    /// </summary>
    public int MovementCount { get; set; }

    /// <summary>
    /// (Classical music only) The movement name of this song.
    /// </summary>
    public string MovementName { get; set; }

    /// <summary>
    /// (Classical music only) The movement number of this song.
    /// </summary>
    public int MovementNumber { get; set; }

    /// <summary>
    /// (Required) The localized name of the song.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The parameters to use to playback the song.
    /// </summary>
    public PlayParameters PlayParams { get; set; }

    /// <summary>
    /// (Required) The preview assets for the song.
    /// </summary>
    public List<Preview> Previews { get; set; }

    /// <summary>
    /// (Required) The release date of the song in YYYY-MM-DD format.
    /// </summary>
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// (Required) The number of the song in the album’s track list.
    /// </summary>
    public int TrackNumber { get; set; }

    /// <summary>
    /// (Required) The URL for sharing a song in the iTunes Store.
    /// </summary>
    public Uri Url { get; set; }

    /// <summary>
    /// (Classical music only) The name of the associated work.
    /// </summary>
    public string WorkName { get; set; }
}