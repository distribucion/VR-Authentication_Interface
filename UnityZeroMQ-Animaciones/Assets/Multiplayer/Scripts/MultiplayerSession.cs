using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MultiplayerSession
{

    public static int CurrentUserIndex = 0;
    public static int CurrentUserId = 0;
    public static string CurrentAvatar => avatarUrls[CurrentUserIndex];

    [SerializeField]
    [Tooltip("Set this to the URL or shortcodes of the Ready Player Me Avatar you want to load.")]
    private static string[] avatarUrls =
        {
            "https://models.readyplayer.me/64bfc617fffca9bd5d533218.glb",
            "https://api.readyplayer.me/v1/avatars/638df5fc5a7d322604bb3a58.glb",
            "https://api.readyplayer.me/v1/avatars/638df70ed72bffc6fa179596.glb",
            "https://api.readyplayer.me/v1/avatars/638df75e5a7d322604bb3dcd.glb",
            "https://api.readyplayer.me/v1/avatars/638df7d1d72bffc6fa179763.glb"
        };

    public static string GetAvatar(int id)
    {
        return avatarUrls[id];
    }


}
