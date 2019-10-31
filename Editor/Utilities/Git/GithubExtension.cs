
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_2019_1_OR_NEWER && UNITY_EDITOR && USE_GITHUB_EXTENSION
using System.Linq;
using UnityEngine.UIElements;

namespace droid.Editor.Utilities.Git {
  /// <summary>
  ///
  /// </summary>
  public static class GithubExtension {
    const string _k_display_none = "display-none";

    /// <summary>
    ///
    /// </summary>
    /// <param name="element"></param>
    /// <param name="value"></param>
    public static void SetElementDisplay(VisualElement element, bool value) {
      if (element == null) {
        return;
      }

      SetElementClass(element, _k_display_none, !value);
      element.visible = value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static bool IsElementDisplay(VisualElement element) {
      return !HasElementClass(element, _k_display_none);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="element"></param>
    /// <param name="class_name"></param>
    /// <param name="value"></param>
    public static void SetElementClass(VisualElement element, string class_name, bool value) {
      if (element == null) {
        return;
      }

      if (value) {
        element.AddToClassList(class_name);
      } else {
        element.RemoveFromClassList(class_name);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="element"></param>
    /// <param name="class_name"></param>
    /// <returns></returns>
    public static bool HasElementClass(VisualElement element, string class_name) {
      if (element == null) {
        return false;
      }

      return element.ClassListContains(class_name);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="package_info"></param>
    /// <returns></returns>
    public static string GetRepoUrl(PackageInfo package_info) {
      return GetRepoUrl(package_info != null ? package_info.packageId : "");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="package_id"></param>
    /// <returns></returns>
    public static string GetRepoUrl(string package_id) {
      var m = Regex.Match(package_id, "^[^@]+@([^#]+)(#.+)?$");
      if (m.Success) {
        var repo_url = m.Groups[1].Value;
        repo_url = Regex.Replace(repo_url, "(git:)?git@([^:]+):", "https://$2/");
        //repoUrl = repoUrl.Replace ("github.com:", "https://github.com/");
        repo_url = repo_url.Replace("ssh://", "https://");
        repo_url = repo_url.Replace("git@", "");
        repo_url = Regex.Replace(repo_url, "\\.git$", "");

        return repo_url;
      }

      return "";
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="package_info"></param>
    /// <returns></returns>
    public static string GetRepoId(PackageInfo package_info) {
      return GetRepoId(package_info != null ? package_info.packageId : "");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="package_id"></param>
    /// <returns></returns>
    public static string GetRepoId(string package_id) {
      var m = Regex.Match(GetRepoUrl(package_id), "/([^/]+/[^/]+)$");
      if (m.Success) {
        return m.Groups[1].Value;
      }

      return "";
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="package_id"></param>
    /// <param name="method_path"></param>
    /// <returns></returns>
    public static string GetApiRequestUrl(string package_id, string method_path) {
      var repo_id = GetRepoId(package_id);
      if (package_id.Contains("github.com")) {
        return "https://api.github.com/repos/" + repo_id + "/" + method_path;
      }

      return "";
    }

    public static AsyncOperation RequestTags(string package_id, List<string> result) {
      return Request(GetApiRequestUrl(package_id, "tags"), x => FillRefNamesFromResponse(x, result));
    }

    public static AsyncOperation RequestBranches(string package_id, List<string> result) {
      return Request(GetApiRequestUrl(package_id, "branches"), x => FillRefNamesFromResponse(x, result));
    }

    public static void FillRefNamesFromResponse(string res, List<string> result) {
      result.Clear();
      result.AddRange(Regex.Matches(res, "\\s*\"name\": \"(.+)\",").Cast<Match>()
                           .Select(x => x.Groups[1].Value));
    }

    public static string GetRevisionHash(PackageInfo package_info) {
      return GetRevisionHash(package_info != null ? package_info.resolvedPath : "");
    }

    public static string GetRevisionHash(string resolved_path) {
      var m = Regex.Match(resolved_path, "@([^@]+)$");
      if (m.Success) {
        return m.Groups[1].Value;
      }

      return "";
    }

    public static string GetFileUrl(PackageInfo package_info, string file_path) {
      return package_info != null
                 ? GetFileUrl(package_info.packageId, package_info.resolvedPath, file_path)
                 : "";
    }

    public static string GetFileUrl(string package_id, string resolved_path, string file_path) {
      if (string.IsNullOrEmpty(package_id)
          || string.IsNullOrEmpty(resolved_path)
          || string.IsNullOrEmpty(file_path)) {
        return "";
      }

      var repo_url = GetRepoUrl(package_id);
      var hash = GetRevisionHash(resolved_path);
      var blob = "blob";

      return $"{repo_url}/{blob}/{hash}/{file_path}";
    }

    public static string GetSpecificPackageId(string package_id, string tag) {
      if (string.IsNullOrEmpty(package_id)) {
        return "";
      }

      var m = Regex.Match(package_id, "^([^#]+)(#.+)?$");
      if (m.Success) {
        var id = m.Groups[1].Value;
        return string.IsNullOrEmpty(tag) ? id : id + "#" + tag;
      }

      return "";
    }

    public static string GetRequestCache(string url) {
      var path = GetRequestCachePath(url);
      return File.Exists(path) && (DateTime.UtcNow - File.GetLastWriteTimeUtc(path)).TotalSeconds < 300
                 ? File.ReadAllText(path)
                 : null;
    }

    public static string GetRequestCachePath(string url) { return "Temp/RequestCache_" + url.GetHashCode(); }

    public static AsyncOperation Request(string url, Action<string> on_success) {
      if (string.IsNullOrEmpty(url)) {
        return null;
      }

      var cache = GetRequestCache(url);
      if (!string.IsNullOrEmpty(cache)) {
        on_success(cache);
        return null;
      }

      var www = UnityWebRequest.Get(url);
      var op = www.SendWebRequest();
      op.completed += _ => {
                        if (www.isHttpError || www.isHttpError || !string.IsNullOrEmpty(www.error)) {
                          Debug.LogError(www.error);
                          www.Dispose();
                          return;
                        }

                        var path = GetRequestCachePath(url);
                        File.WriteAllText(path, www.downloadHandler.text);
                        on_success(www.downloadHandler.text);
                        www.Dispose();
                      };
      return op;
    }
  }
}
#endif
