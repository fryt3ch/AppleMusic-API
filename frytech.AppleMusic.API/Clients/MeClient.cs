﻿using System.Collections.Specialized;
using frytech.AppleMusic.API.Clients.Interfaces;
using frytech.AppleMusic.API.Configuration;
using frytech.AppleMusic.API.Models.Core;
using frytech.AppleMusic.API.Models.Enums;
using frytech.AppleMusic.API.Models.Enums.Relationships;
using frytech.AppleMusic.API.Models.Requests;
using frytech.AppleMusic.API.Models.Responses;
using frytech.AppleMusic.API.Extensions;
using Microsoft.Extensions.Options;

namespace frytech.AppleMusic.API.Clients;

public class MeClient : BaseClient, IMeClient
{
    private const string BaseRequestUri = "me";

    public MeClient(HttpClient client, IOptions<AppleMusicClientConfiguration> options) : base(client, options)
    {
    }

    #region Route: me/history/heavy-rotation

    /// <summary>
    /// Fetch the resources in heavy rotation for the user.
    /// Route: me/history/heavy-rotation
    /// https://developer.apple.com/documentation/applemusicapi/get_heavy_rotation_content
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<HistoryResponse> GetHeavyRotationContent(string userToken, PageOptions? pageOptions = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        return await Get<HistoryResponse>($"{BaseRequestUri}/history/heavy-rotation", pageOptions: pageOptions, locale: locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library

    /// <summary>
    /// Add a catalog resource to a user’s iCloud Music Library.
    /// POST me/library
    /// https://developer.apple.com/documentation/applemusicapi/add_a_resource_to_a_library
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<ResponseRoot> AddResourceToLibrary(string userToken, IReadOnlyDictionary<iCloudMusicLibraryType, List<string>> ids)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any(x => x.Value.Any()))
            throw new ArgumentNullException(nameof(ids));

        SetUserTokenHeader(userToken);

        var queryString = ids
            .Where(x => x.Value.Any(y => !string.IsNullOrWhiteSpace(y)))
            .ToNameValueCollection(x => $"ids[{x.Key.GetValue()}]", x => string.Join(",", x.Value.Where(y => !string.IsNullOrWhiteSpace(y))));

        return await Post<ResponseRoot>($"{BaseRequestUri}/library", queryString);
    }

    #endregion

    #region Route: me/library/albums/{id}

    /// <summary>
    /// Fetch a library album by using its identifier.
    /// Route: me/library/albums/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_library_album
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryAlbumResponse> GetLibraryAlbum(string userToken, string id, IReadOnlyCollection<LibraryAlbumRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        SetUserTokenHeader(userToken);

        return await GetLibraryResource<LibraryAlbumResponse, LibraryAlbumRelationshipType>(ResourceType.Albums, id, include, locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/albums/{id}/{relationship}

    /// <summary>
    /// Fetch a library album's relationship by using its identifier.
    /// Route: me/library/albums/{id}/{relationship}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_library_album_s_relationship_directly_by_name
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="relationshipType"></param>
    /// <param name="pageOptions"></param>
    /// <returns></returns>
    public async Task<LibraryAlbumResponse> GetLibraryAlbumRelationship(string userToken, string id, LibraryAlbumRelationshipType relationshipType, PageOptions? pageOptions = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        SetUserTokenHeader(userToken);

        return await GetLibraryResourceRelationship<LibraryAlbumResponse, LibraryAlbumRelationshipType>(ResourceType.Albums, id, relationshipType, pageOptions)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/albums

    /// <summary>
    /// Fetch one or more library albums by using their identifiers.
    /// Route: me/library/albums
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_library_albums
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryAlbumResponse> GetMultipleLibraryAlbums(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<LibraryAlbumRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        SetUserTokenHeader(userToken);

        return await GetMultipleLibraryResources<LibraryAlbumResponse, LibraryAlbumRelationshipType>(ResourceType.Albums, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch all the library albums in alphabetical order.
    /// Route: me/library/albums
    /// https://developer.apple.com/documentation/applemusicapi/get_all_library_albums
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="include"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryAlbumResponse> GetAllLibraryAlbums(string userToken, IReadOnlyCollection<LibraryAlbumRelationshipType>? include = null, PageOptions? pageOptions = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        return await GetAllLibraryResources<LibraryAlbumResponse, LibraryAlbumRelationshipType>(ResourceType.Albums, include, pageOptions, locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/artists/{id}

    /// <summary>
    /// Fetch a library artist by using its identifier.
    /// Route: me/library/artists/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_library_artist
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryArtistResponse> GetLibraryArtist(string userToken, string id, IReadOnlyCollection<LibraryArtistRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        SetUserTokenHeader(userToken);

        return await GetLibraryResource<LibraryArtistResponse, LibraryArtistRelationshipType>(ResourceType.Artists, id, include, locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/artists/{id}/{relationship}

    /// <summary>
    /// Fetch a library artist's relationship by using its identifier.
    /// Route: me/library/artists/{id}/{relationship}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_library_artist_s_relationship_directly_by_name
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="relationshipType"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<LibraryArtistResponse> GetLibraryArtistRelationship(string userToken, string id, LibraryArtistRelationshipType relationshipType, int? limit = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        SetUserTokenHeader(userToken);

        return await GetLibraryResourceRelationship<LibraryArtistResponse, LibraryArtistRelationshipType>(ResourceType.Artists, id, relationshipType, limit)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/artists

    /// <summary>
    /// Fetch one or more library artists by using their identifiers.
    /// Route: me/library/artists
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_library_artists
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryArtistResponse> GetMultipleLibraryArtists(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<LibraryArtistRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        SetUserTokenHeader(userToken);

        return await GetMultipleLibraryResources<LibraryArtistResponse, LibraryArtistRelationshipType>(ResourceType.Artists, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch all the library artists in alphabetical order.
    /// Route: me/library/artists
    /// https://developer.apple.com/documentation/applemusicapi/get_all_library_artists
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="include"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryArtistResponse> GetAllLibraryArtists(string userToken, IReadOnlyCollection<LibraryArtistRelationshipType>? include = null, PageOptions? pageOptions = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        return await GetAllLibraryResources<LibraryArtistResponse, LibraryArtistRelationshipType>(ResourceType.Artists, include, pageOptions, locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/music-videos/{id}

    /// <summary>
    /// Fetch a library music video by using its identifier.
    /// Route: me/library/music-videos/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_library_music_video
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryMusicVideoResponse> GetLibraryMusicVideo(string userToken, string id, IReadOnlyCollection<LibraryMusicVideoRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        SetUserTokenHeader(userToken);

        return await GetLibraryResource<LibraryMusicVideoResponse, LibraryMusicVideoRelationshipType>(ResourceType.MusicVideos, id, include, locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/music-videos/{id}/{relationship}

    /// <summary>
    /// Fetch a library music video's relationship by using its identifier.
    /// Route: me/library/music-videos/{id}/{relationship}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_library_music_video_s_relationship_directly_by_name
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="relationshipType"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<LibraryMusicVideoResponse> GetLibraryMusicVideoRelationship(string userToken, string id, LibraryMusicVideoRelationshipType relationshipType, int? limit = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        SetUserTokenHeader(userToken);

        return await GetLibraryResourceRelationship<LibraryMusicVideoResponse, LibraryMusicVideoRelationshipType>(ResourceType.MusicVideos, id, relationshipType, limit)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/music-videos

    /// <summary>
    /// Fetch one or more library music videos by using their identifiers.
    /// Route: me/library/music-videos
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_library_music_videos
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryMusicVideoResponse> GetMultipleLibraryMusicVideos(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<LibraryMusicVideoRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        SetUserTokenHeader(userToken);

        return await GetMultipleLibraryResources<LibraryMusicVideoResponse, LibraryMusicVideoRelationshipType>(ResourceType.MusicVideos, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch all the library music videos in alphabetical order.
    /// Route: me/library/music-videos
    /// https://developer.apple.com/documentation/applemusicapi/get_all_library_music_videos
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="include"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryMusicVideoResponse> GetAllLibraryMusicVideos(string userToken, IReadOnlyCollection<LibraryMusicVideoRelationshipType>? include = null, PageOptions? pageOptions = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        return await GetAllLibraryResources<LibraryMusicVideoResponse, LibraryMusicVideoRelationshipType>(ResourceType.MusicVideos, include, pageOptions, locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/playlists/{id}

    /// <summary>
    /// Fetch a library playlist by using its identifier.
    /// Route: me/library/playlists/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_library_playlist
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryPlaylistResponse> GetLibraryPlaylist(string userToken, string id, IReadOnlyCollection<LibraryPlaylistRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        SetUserTokenHeader(userToken);

        return await GetLibraryResource<LibraryPlaylistResponse, LibraryPlaylistRelationshipType>(ResourceType.Playlists, id, include, locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/playlists/{id}/{relationship}

    /// <summary>
    /// Fetch a library playlist's relationship by using its identifier.
    /// Route: me/library/playlists/{id}/{relationship}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_library_playlist_s_relationship_directly_by_name
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="relationshipType"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<LibraryPlaylistResponse> GetLibraryPlaylistRelationship(string userToken, string id, LibraryPlaylistRelationshipType relationshipType, int? limit = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        SetUserTokenHeader(userToken);

        return await GetLibraryResourceRelationship<LibraryPlaylistResponse, LibraryPlaylistRelationshipType>(ResourceType.Playlists, id, relationshipType, limit)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/playlists

    /// <summary>
    /// Fetch one or more library playlists by using their identifiers.
    /// Route: me/library/playlists
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_library_playlists
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryPlaylistResponse> GetMultipleLibraryPlaylists(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<LibraryPlaylistRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        SetUserTokenHeader(userToken);

        return await GetMultipleLibraryResources<LibraryPlaylistResponse, LibraryPlaylistRelationshipType>(ResourceType.Playlists, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch all the library playlists in alphabetical order.
    /// https://developer.apple.com/documentation/applemusicapi/get_all_library_playlists
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="include"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryPlaylistResponse> GetAllLibraryPlaylists(string userToken, IReadOnlyCollection<LibraryPlaylistRelationshipType>? include = null, PageOptions? pageOptions = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        return await GetAllLibraryResources<LibraryPlaylistResponse, LibraryPlaylistRelationshipType>(ResourceType.Playlists, include, pageOptions, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Create a new playlist in a user’s library.
    /// POST me/library/playlists
    /// https://developer.apple.com/documentation/applemusicapi/create_a_new_library_playlist
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="request"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibraryPlaylistResponse> CreateLibraryPlaylist(string userToken, LibraryPlaylistCreationRequest request, IReadOnlyCollection<LibraryPlaylistRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        var queryString = new NameValueCollection();
        
        if (include != null && include.Any())
            queryString.Add("include", string.Join(",", include.Select(x => x.GetValue())));

        return await Post<LibraryPlaylistResponse, LibraryPlaylistCreationRequest>($"{BaseRequestUri}/library/playlists", request, queryString, locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/recently-added

    /// <summary>
    /// Fetch the resources recently added to the library.
    /// Route: me/library/recently-added
    /// https://developer.apple.com/documentation/applemusicapi/get_recently_added_resources
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="pageOptions"></param>
    /// <returns></returns>
    public async Task<RecentlyAddedResponse> GetRecentlyAddedResources(string userToken, PageOptions? pageOptions = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        return await Get<RecentlyAddedResponse>($"{BaseRequestUri}/library/recently-added", pageOptions: pageOptions)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/search

    /// <summary>
    /// Search the library by using a query.
    /// Route: me/library/search
    /// https://developer.apple.com/documentation/applemusicapi/search_for_library_resources
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="term"></param>
    /// <param name="types"></param>
    /// <param name="pageOptions"></param>
    /// <returns></returns>
    public async Task<LibrarySearchResponse> LibraryResourcesSearch(string userToken, string? term = null, IReadOnlyCollection<ResourceType>? types = null, PageOptions? pageOptions = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        var queryString = new NameValueCollection();

        if (!string.IsNullOrWhiteSpace(term))
            queryString.Add("term", term);

        if (types != null && types.Any())
            queryString.Add("types", string.Join(",", types.Select(x => x.GetValue())));

        return await Get<LibrarySearchResponse>($"{BaseRequestUri}/library/search", queryString, pageOptions)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/songs/{id}

    /// <summary>
    /// Fetch a library song by using its identifier.
    /// https://developer.apple.com/documentation/applemusicapi/get_a_library_song
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibrarySongResponse> GetLibrarySong(string userToken, string id, IReadOnlyCollection<LibrarySongRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        SetUserTokenHeader(userToken);

        return await GetLibraryResource<LibrarySongResponse, LibrarySongRelationshipType>(ResourceType.Songs, id, include, locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/songs/{id}/{relationship}

    /// <summary>
    /// Fetch a library song's relationship by using its identifier.
    /// https://developer.apple.com/documentation/applemusicapi/get_a_library_song_s_relationship_directly_by_name
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="relationshipType"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<LibrarySongResponse> GetLibrarySongRelationship(string userToken, string id, LibrarySongRelationshipType relationshipType, int? limit = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        SetUserTokenHeader(userToken);

        return await GetLibraryResourceRelationship<LibrarySongResponse, LibrarySongRelationshipType>(ResourceType.Songs, id, relationshipType, limit)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/library/songs

    /// <summary>
    /// Fetch a library song by using its identifier.
    /// https://developer.apple.com/documentation/applemusicapi/get_a_library_song
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibrarySongResponse> GetMultipleLibrarySongs(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<LibrarySongRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        SetUserTokenHeader(userToken);

        return await GetMultipleLibraryResources<LibrarySongResponse, LibrarySongRelationshipType>(ResourceType.Songs, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch all the library songs in alphabetical order.
    /// https://developer.apple.com/documentation/applemusicapi/get_all_library_songs
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="include"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<LibrarySongResponse> GetAllLibrarySongs(string userToken, IReadOnlyCollection<LibrarySongRelationshipType>? include = null, PageOptions? pageOptions = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        return await GetAllLibraryResources<LibrarySongResponse, LibrarySongRelationshipType>(ResourceType.Songs, include, pageOptions, locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/ratings/albums

    /// <summary>
    /// Fetch a user’s rating for an album by using the user's identifier.
    /// Route: me/ratings/albums/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_personal_album_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetAlbumRating(string userToken, string id, IReadOnlyCollection<AlbumRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await GetResourceRating(userToken, ResourceType.Albums, id, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch the user’s ratings for one or more albums by using the albums' identifiers.
    /// Route: me/ratings/albums
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_personal_album_ratings
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetMultipleAlbumRatings(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<AlbumRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        return await GetMultipleResourceRatings(userToken, ResourceType.Albums, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Add a user’s album rating by using the album's identifier.
    /// Route: me/ratings/albums/{id}
    /// https://developer.apple.com/documentation/applemusicapi/add_a_personal_album_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<RatingResponse> AddAlbumRating(string userToken, string id, RatingRequest request)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return await AddResourceRating(userToken, ResourceType.Albums, id, request)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Remove a user’s album rating by using the album's identifier.
    /// Route: me/ratings/albums/{id}
    /// https://developer.apple.com/documentation/applemusicapi/delete_a_personal_album_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseRoot> DeleteAlbumRating(string userToken, string id)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await DeleteResourceRating(userToken, ResourceType.Albums, id)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/ratings/music-videos

    /// <summary>
    /// Fetch a user’s rating for a music video by using the video's identifier.
    /// Route: me/ratings/music-videos/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_personal_music_video_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetMusicVideoRating(string userToken, string id, IReadOnlyCollection<MusicVideoRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await GetResourceRating(userToken, ResourceType.MusicVideos, id, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch the user’s ratings for one or more music videos by using the music videos' identifiers.
    /// Route: me/ratings/music-videos
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_personal_music_video_ratings
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetMultipleMusicVideoRatings(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<MusicVideoRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        return await GetMultipleResourceRatings(userToken, ResourceType.MusicVideos, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Add a user’s music video rating by using the music video's identifier.
    /// Route: me/ratings/music-videos/{id}
    /// https://developer.apple.com/documentation/applemusicapi/add_a_personal_music_video_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<RatingResponse> AddMusicVideoRating(string userToken, string id, RatingRequest request)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return await AddResourceRating(userToken, ResourceType.MusicVideos, id, request)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Remove a user’s music video rating by using the music video's identifier.
    /// Route: me/ratings/music-videos/{id}
    /// https://developer.apple.com/documentation/applemusicapi/delete_a_personal_music_video_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseRoot> DeleteMusicVideoRating(string userToken, string id)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await DeleteResourceRating(userToken, ResourceType.MusicVideos, id)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/ratings/playlists

    /// <summary>
    /// Fetch a user’s rating for a playlist by using the playlist's identifier.
    /// Route: me/ratings/playlists/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_personal_playlist_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetPlaylistRating(string userToken, string id, IReadOnlyCollection<PlaylistRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await GetResourceRating(userToken, ResourceType.Playlists, id, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch the user’s ratings for one or more playlists by using the playlists' identifiers.
    /// Route: me/ratings/playlists
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_personal_playlist_ratings
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetMultiplePlaylistRatings(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<PlaylistRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        return await GetMultipleResourceRatings(userToken, ResourceType.Playlists, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Add a user’s playlist rating by using the playlist's identifier.
    /// Route: me/ratings/playlists/{id}
    /// https://developer.apple.com/documentation/applemusicapi/add_a_personal_playlist_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<RatingResponse> AddPlaylistRating(string userToken, string id, RatingRequest request)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return await AddResourceRating(userToken, ResourceType.Playlists, id, request)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Remove a user’s playlist rating by using the playlist's identifier.
    /// Route: me/ratings/playlists/{id}
    /// https://developer.apple.com/documentation/applemusicapi/delete_a_personal_playlist_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseRoot> DeletePlaylistRating(string userToken, string id)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await DeleteResourceRating(userToken, ResourceType.Playlists, id)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/ratings/songs

    /// <summary>
    /// Fetch a user’s rating for a song by using the song's identifier.
    /// Route: me/ratings/songs/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_personal_song_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetSongRating(string userToken, string id, IReadOnlyCollection<SongRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await GetResourceRating(userToken, ResourceType.Songs, id, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch the user’s ratings for one or more songs by using the songs' identifiers.
    /// Route: me/ratings/songs
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_personal_song_ratings
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetMultipleSongRatings(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<SongRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        return await GetMultipleResourceRatings(userToken, ResourceType.Songs, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Add a user’s song rating by using the song's identifier.
    /// Route: me/ratings/songs/{id}
    /// https://developer.apple.com/documentation/applemusicapi/add_a_personal_song_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<RatingResponse> AddSongRating(string userToken, string id, RatingRequest request)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return await AddResourceRating(userToken, ResourceType.Songs, id, request)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Remove a user’s song rating by using the song's identifier.
    /// Route: me/ratings/songs/{id}
    /// https://developer.apple.com/documentation/applemusicapi/delete_a_personal_song_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseRoot> DeleteSongRating(string userToken, string id)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await DeleteResourceRating(userToken, ResourceType.Songs, id)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/ratings/stations

    /// <summary>
    /// Fetch a user’s rating for a station by using the station's identifier.
    /// Route: me/ratings/stations/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_personal_station_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetStationRating(string userToken, string id, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await GetResourceRating(userToken, ResourceType.Stations, id, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch the user’s ratings for one or more stations by using the stations' identifiers.
    /// Route: me/ratings/stations
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_personal_station_ratings
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetMultipleStationRatings(string userToken, IReadOnlyCollection<string> ids, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        return await GetMultipleResourceRatings(userToken, ResourceType.Stations, ids, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Add a user’s station rating by using the station's identifier.
    /// Route: me/ratings/stations/{id}
    /// https://developer.apple.com/documentation/applemusicapi/add_a_personal_station_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<RatingResponse> AddStationRating(string userToken, string id, RatingRequest request)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return await AddResourceRating(userToken, ResourceType.Stations, id, request)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Remove a user’s station rating by using the station's identifier.
    /// Route: me/ratings/stations/{id}
    /// https://developer.apple.com/documentation/applemusicapi/delete_a_personal_station_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseRoot> DeleteStationRating(string userToken, string id)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await DeleteResourceRating(userToken, ResourceType.Stations, id)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/ratings/library-music-videos

    /// <summary>
    /// Fetch a user’s rating for a library music video by using the music video's library identifier.
    /// Route: me/ratings/library-music-videos/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_personal_library_music_video_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetLibraryMusicVideoRating(string userToken, string id, IReadOnlyCollection<LibraryMusicVideoRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await GetResourceRating(userToken, ResourceType.LibraryMusicVideos, id, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch the user’s ratings for one or more library music videos by using the library music videos' identifiers.
    /// Route: me/ratings/library-music-videos
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_personal_library_music_video_ratings
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetMultipleLibraryMusicVideoRatings(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<LibraryMusicVideoRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        return await GetMultipleResourceRatings(userToken, ResourceType.LibraryMusicVideos, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Add a user’s library music video rating by using the library music video's identifier.
    /// Route: me/ratings/library-music-videos/{id}
    /// https://developer.apple.com/documentation/applemusicapi/add_a_personal_library_music_video_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<RatingResponse> AddLibraryMusicVideoRating(string userToken, string id, RatingRequest request)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return await AddResourceRating(userToken, ResourceType.LibraryMusicVideos, id, request)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Remove a user’s library music video rating by using the library music video's identifier.
    /// Route: me/ratings/library-music-videos/{id}
    /// https://developer.apple.com/documentation/applemusicapi/delete_a_personal_library_music_video_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseRoot> DeleteLibraryMusicVideoRating(string userToken, string id)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await DeleteResourceRating(userToken, ResourceType.LibraryMusicVideos, id)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/ratings/library-playlists

    /// <summary>
    /// Fetch a user’s rating for a library playlist by using the playlist's library identifier.
    /// Route: me/ratings/library-playlists/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_personal_library_playlist_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetLibraryPlaylistRating(string userToken, string id, IReadOnlyCollection<LibraryPlaylistRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await GetResourceRating(userToken, ResourceType.LibraryPlaylists, id, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch the user’s ratings for one or more library playlists by using the library playlists' identifiers.
    /// Route: me/ratings/library-playlists
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_personal_library_playlist_ratings
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetMultipleLibraryPlaylistRatings(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<LibraryPlaylistRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        return await GetMultipleResourceRatings(userToken, ResourceType.LibraryPlaylists, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Add a user’s library playlist rating by using the library playlist's identifier.
    /// Route: me/ratings/library-playlists/{id}
    /// https://developer.apple.com/documentation/applemusicapi/add_a_personal_library_playlist_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<RatingResponse> AddLibraryPlaylistRating(string userToken, string id, RatingRequest request)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return await AddResourceRating(userToken, ResourceType.LibraryPlaylists, id, request)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Remove a user’s library playlist rating by using the library playlist's identifier.
    /// Route: me/ratings/library-playlists/{id}
    /// https://developer.apple.com/documentation/applemusicapi/delete_a_personal_library_playlist_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseRoot> DeleteLibraryPlaylistRating(string userToken, string id)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await DeleteResourceRating(userToken, ResourceType.LibraryPlaylists, id)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/ratings/library-songs

    /// <summary>
    /// Fetch a user’s rating for a library song by using the song's library identifier.
    /// Route: me/ratings/library-songs/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_personal_library_song_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetLibrarySongRating(string userToken, string id, IReadOnlyCollection<LibrarySongRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await GetResourceRating(userToken, ResourceType.LibrarySongs, id, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch the user’s ratings for one or more library songs by using the library songs' identifiers.
    /// Route: me/ratings/library-songs
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_personal_library_songs_ratings
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RatingResponse> GetMultipleLibrarySongRatings(string userToken, IReadOnlyCollection<string> ids, IReadOnlyCollection<LibrarySongRelationshipType>? include = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        return await GetMultipleResourceRatings(userToken, ResourceType.LibrarySongs, ids, include, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Add a user’s library song rating by using the library song's identifier.
    /// Route: me/ratings/library-songs/{id}
    /// https://developer.apple.com/documentation/applemusicapi/add_a_personal_library_song_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<RatingResponse> AddLibrarySongRating(string userToken, string id, RatingRequest request)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return await AddResourceRating(userToken, ResourceType.LibrarySongs, id, request)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Remove a user’s library song rating by using the library song's identifier.
    /// Route: me/ratings/library-songs/{id}
    /// https://developer.apple.com/documentation/applemusicapi/delete_a_personal_library_song_rating
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseRoot> DeleteLibrarySongRating(string userToken, string id)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        return await DeleteResourceRating(userToken, ResourceType.LibrarySongs, id)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/recent/played

    /// <summary>
    /// Fetch the recently played resources for the user.
    /// Route: me/recent/played
    /// https://developer.apple.com/documentation/applemusicapi/get_recently_played_resources
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<HistoryResponse> GetRecentlyPlayedResources(string userToken, PageOptions? pageOptions = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        return await Get<HistoryResponse>($"{BaseRequestUri}/recent/played", pageOptions: pageOptions, locale: locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/recent/radio-stations

    /// <summary>
    /// Fetch recently played radio stations for the user.
    /// Route: me/recent/radio-stations
    /// https://developer.apple.com/documentation/applemusicapi/get_recently_played_stations
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<HistoryResponse> GetRecentlyPlayedStations(string userToken, PageOptions? pageOptions = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        return await Get<HistoryResponse>($"{BaseRequestUri}/recent/radio-stations", pageOptions: pageOptions, locale: locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/recommendations

    /// <summary>
    /// Fetch a recommendation by using its identifier.
    /// Route: me/recommendations/{id}
    /// https://developer.apple.com/documentation/applemusicapi/get_a_recommendation
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="id"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RecommendationResponse> GetRecommendation(string userToken, string id, PageOptions? pageOptions = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        SetUserTokenHeader(userToken);

        return await Get<RecommendationResponse>($"{BaseRequestUri}/recommendations/{id}", pageOptions: pageOptions, locale: locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch one or more recommendations by using their identifiers.
    /// Route: me/recommendations
    /// https://developer.apple.com/documentation/applemusicapi/get_multiple_recommendations
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="ids"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RecommendationResponse> GetMultipleRecommendations(string userToken, IReadOnlyCollection<string> ids, PageOptions? pageOptions = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        if (ids == null || !ids.Any())
            throw new ArgumentNullException(nameof(ids));

        SetUserTokenHeader(userToken);

        var queryString = new NameValueCollection()
        {
            { "ids", string.Join(",", ids) }
        };

        return await Get<RecommendationResponse>($"{BaseRequestUri}/recommendations", queryString, pageOptions, locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch default recommendations.
    /// Route: me/recommendations
    /// https://developer.apple.com/documentation/applemusicapi/get_default_recommendations
    /// </summary>
    /// <param name="userToken"></param>
    /// <param name="recommendationType"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    public async Task<RecommendationResponse> GetDefaultRecommendations(string userToken, RecommendationType? recommendationType = null, PageOptions? pageOptions = null, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        var queryString = new NameValueCollection();

        if (recommendationType.HasValue)
        {
            queryString.Add("type", recommendationType.Value.GetValue());
        };

        return await Get<RecommendationResponse>($"{BaseRequestUri}/recommendations", queryString, pageOptions, locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Route: me/storefront

    /// <summary>
    /// Fetch a user’s storefront.
    /// Route: me/storefront
    /// https://developer.apple.com/documentation/applemusicapi/get_a_user_s_storefront
    /// </summary>
    /// <param name="userToken"></param>
    /// <returns></returns>
    public async Task<StorefrontResponse> GetUsersStorefront(string userToken, string? locale = null)
    {
        if (string.IsNullOrWhiteSpace(userToken))
            throw new ArgumentNullException(nameof(userToken));

        SetUserTokenHeader(userToken);

        return await Get<StorefrontResponse>($"{BaseRequestUri}/storefront", locale: locale)
            .ConfigureAwait(false);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Fetch a resource rating.
    /// For resources without relationships.
    /// </summary>
    /// <typeparam name="TResourceEnum"></typeparam>
    /// <param name="userToken"></param>
    /// <param name="resource"></param>
    /// <param name="id"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    private async Task<RatingResponse> GetResourceRating<TResourceEnum>(string userToken, TResourceEnum resource, string id, string? locale = null)
        where TResourceEnum : IConvertible
    {
        SetUserTokenHeader(userToken);

        return await Get<RatingResponse>($"{BaseRequestUri}/ratings/{resource.GetValue()}/{id}", locale: locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch a resource rating.
    /// </summary>
    /// <typeparam name="TResourceEnum"></typeparam>
    /// <typeparam name="TRelationshipEnum"></typeparam>
    /// <param name="userToken"></param>
    /// <param name="resource"></param>
    /// <param name="id"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    private async Task<RatingResponse> GetResourceRating<TResourceEnum, TRelationshipEnum>(string userToken, TResourceEnum resource, string id, IReadOnlyCollection<TRelationshipEnum>? include, string? locale = null)
        where TResourceEnum : IConvertible
        where TRelationshipEnum : IConvertible
    {
        SetUserTokenHeader(userToken);

        var queryString = new NameValueCollection();
        
        if (include != null && include.Any())
            queryString.Add("include", string.Join(",", include.Select(x => x.GetValue())));

        return await Get<RatingResponse>($"{BaseRequestUri}/ratings/{resource.GetValue()}/{id}", queryString, locale: locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch multiple resource ratings.
    /// For resources without relationships.
    /// </summary>
    /// <typeparam name="TResourceEnum"></typeparam>
    /// <param name="userToken"></param>
    /// <param name="resource"></param>
    /// <param name="ids"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    private async Task<RatingResponse> GetMultipleResourceRatings<TResourceEnum>(string userToken, TResourceEnum resource, IReadOnlyCollection<string> ids, string? locale = null)
        where TResourceEnum : IConvertible
    {
        SetUserTokenHeader(userToken);

        var queryString = new NameValueCollection()
        {
            { "ids", string.Join(",", ids) }
        };

        return await Get<RatingResponse>($"{BaseRequestUri}/ratings/{resource.GetValue()}", queryString, locale: locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch multiple resource ratings.
    /// </summary>
    /// <typeparam name="TResourceEnum"></typeparam>
    /// <typeparam name="TRelationshipEnum"></typeparam>
    /// <param name="userToken"></param>
    /// <param name="resource"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    private async Task<RatingResponse> GetMultipleResourceRatings<TResourceEnum, TRelationshipEnum>(string userToken, TResourceEnum resource, IReadOnlyCollection<string> ids, IReadOnlyCollection<TRelationshipEnum>? include = null, string? locale = null)
        where TResourceEnum : IConvertible
        where TRelationshipEnum : IConvertible
    {
        SetUserTokenHeader(userToken);

        var queryString = new NameValueCollection()
        {
            { "ids", string.Join(",", ids) }
        };

        if (include != null && include.Any())
            queryString.Add("include", string.Join(",", include.Select(x => x.GetValue())));

        return await Get<RatingResponse>($"{BaseRequestUri}/ratings/{resource.GetValue()}", queryString, locale: locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Add a resource rating.
    /// </summary>
    /// <typeparam name="TResourceEnum"></typeparam>
    /// <param name="userToken"></param>
    /// <param name="resource"></param>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<RatingResponse> AddResourceRating<TResourceEnum>(string userToken, TResourceEnum resource, string id, RatingRequest request)
        where TResourceEnum : IConvertible
    {
        SetUserTokenHeader(userToken);

        return await Put<RatingResponse, RatingRequest>($"{BaseRequestUri}/ratings/{resource.GetValue()}/{id}", request)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Delete a resource rating.
    /// </summary>
    /// <typeparam name="TResourceEnum"></typeparam>
    /// <param name="userToken"></param>
    /// <param name="resource"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<ResponseRoot> DeleteResourceRating<TResourceEnum>(string userToken, TResourceEnum resource, string id)
        where TResourceEnum : IConvertible
    {
        SetUserTokenHeader(userToken);

        return await Delete($"{BaseRequestUri}/ratings/{resource.GetValue()}/{id}")
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch a library resource by using its identifier.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TRelationshipEnum"></typeparam>
    /// <param name="libraryResource"></param>
    /// <param name="id"></param>
    /// <param name="relationshipsToInclude"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    private async Task<TResponse> GetLibraryResource<TResponse, TRelationshipEnum>(ResourceType libraryResource, string id, IReadOnlyCollection<TRelationshipEnum>? relationshipsToInclude = null, string? locale = null)
        where TRelationshipEnum : IConvertible
    {
        var queryString = new NameValueCollection();
        
        if (relationshipsToInclude != null && relationshipsToInclude.Any())
            queryString.Add("include", string.Join(",", relationshipsToInclude.Select(x => x.GetValue())));

        return await Get<TResponse>($"{BaseRequestUri}/library/{libraryResource.GetValue()}/{id}", queryString, locale: locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch a library resource's relationship by using its identifier.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TRelationshipEnum"></typeparam>
    /// <param name="libraryResource"></param>
    /// <param name="id"></param>
    /// <param name="relationship"></param>
    /// <param name="pageOptions"></param>
    /// <returns></returns>
    private async Task<TResponse> GetLibraryResourceRelationship<TResponse, TRelationshipEnum>(ResourceType libraryResource, string id, TRelationshipEnum relationship, PageOptions? pageOptions = null)
        where TRelationshipEnum : IConvertible
    {
        return await Get<TResponse>($"{BaseRequestUri}/library/{libraryResource.GetValue()}/{id}/{relationship.GetValue()}", pageOptions: pageOptions)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch a library resource's relationship by using its identifier.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TRelationshipEnum"></typeparam>
    /// <param name="libraryResource"></param>
    /// <param name="id"></param>
    /// <param name="relationship"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    private async Task<TResponse> GetLibraryResourceRelationship<TResponse, TRelationshipEnum>(ResourceType libraryResource, string id, TRelationshipEnum relationship, int? limit = null)
        where TRelationshipEnum : IConvertible
    {
        var queryString = new NameValueCollection();
        
        if (limit.HasValue)
            queryString.Add("limit", limit.Value.ToString());

        return await Get<TResponse>($"{BaseRequestUri}/library/{libraryResource.GetValue()}/{id}/{relationship.GetValue()}", queryString)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch one or more library resources by using their identifiers.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TRelationshipEnum"></typeparam>
    /// <param name="libraryResource"></param>
    /// <param name="ids"></param>
    /// <param name="include"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    private async Task<TResponse> GetMultipleLibraryResources<TResponse, TRelationshipEnum>(ResourceType libraryResource, IReadOnlyCollection<string>? ids, IReadOnlyCollection<TRelationshipEnum>? include = null, string? locale = null)
        where TRelationshipEnum : IConvertible
    {
        var queryString = new NameValueCollection();

        if (ids != null && ids.Any())
            queryString.Add("ids", string.Join(",", ids));

        if (include != null && include.Any())
            queryString.Add("include", string.Join(",", include.Select(x => x.GetValue())));

        return await Get<TResponse>($"{BaseRequestUri}/library/{libraryResource.GetValue()}", queryString, locale: locale)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Fetch all library resources.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TRelationshipEnum"></typeparam>
    /// <param name="libraryResource"></param>
    /// <param name="include"></param>
    /// <param name="pageOptions"></param>
    /// <param name="locale"></param>
    /// <returns></returns>
    private async Task<TResponse> GetAllLibraryResources<TResponse, TRelationshipEnum>(ResourceType libraryResource, IReadOnlyCollection<TRelationshipEnum>? include = null, PageOptions? pageOptions = null, string? locale = null)
        where TRelationshipEnum : IConvertible
    {
        var queryString = new NameValueCollection();

        if (include != null && include.Any())
            queryString.Add("include", string.Join(",", include.Select(x => x.GetValue())));

        return await Get<TResponse>($"{BaseRequestUri}/library/{libraryResource.GetValue()}", queryString, pageOptions, locale)
            .ConfigureAwait(false); 
    }

    #endregion
}