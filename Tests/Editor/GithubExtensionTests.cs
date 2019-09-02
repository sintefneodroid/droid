#if UNITY_2019_1_OR_NEWER && USE_GITHUB_EXTENSION && False
using System.Collections;
using System.IO;
using System.Linq;
using droid.Editor.Utilities.Git;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace droid.Tests.Editor {
  public class GithubExtensionTests {
    const string _package_name = "com.cnheider.gitpackagetest";
    const string _user_repo = "cnheider/GitPackageTest";
    const string _repo_url = "https://github.com/" + _user_repo;
    const string _revision_hash = "9cd79485e6b8a696aac0e7bb9128ba2926121188";
    const string _file_name = "README.md";
    const string _file_url = _repo_url + "/blob/" + _revision_hash + "/" + _file_name;

    [TestCase("", ExpectedResult = "")]
    [TestCase(_package_name + "@https://github.com/" + _user_repo + ".git", ExpectedResult = _repo_url)]
    [TestCase(_package_name + "@https://github.com/" + _user_repo + ".git#0.3.0", ExpectedResult = _repo_url)]
    [TestCase(_package_name + "@ssh://git@github.com/" + _user_repo + ".git", ExpectedResult = _repo_url)]
    [TestCase(_package_name + "@ssh://git@github.com/" + _user_repo + ".git#0.3.0",
        ExpectedResult = _repo_url)]
    [TestCase(_package_name + "@git@github.com:" + _user_repo + ".git", ExpectedResult = _repo_url)]
    [TestCase(_package_name + "@git@github.com:" + _user_repo + ".git#0.3.0", ExpectedResult = _repo_url)]
    [TestCase(_package_name + "@git:git@github.com:" + _user_repo + ".git", ExpectedResult = _repo_url)]
    [TestCase(_package_name + "@git:git@github.com:" + _user_repo + ".git#0.3.0", ExpectedResult = _repo_url)]
    public string GetRepoUrlTest(string package_id) { return GithubExtension.GetRepoUrl(package_id); }

    [TestCase("", ExpectedResult = "")]
    [TestCase(_package_name + "@https://github.com/" + _user_repo + ".git", ExpectedResult = _user_repo)]
    [TestCase(_package_name + "@https://github.com/" + _user_repo + ".git#0.3.0",
        ExpectedResult = _user_repo)]
    [TestCase(_package_name + "@ssh://git@github.com/" + _user_repo + ".git", ExpectedResult = _user_repo)]
    [TestCase(_package_name + "@ssh://git@github.com/" + _user_repo + ".git#0.3.0",
        ExpectedResult = _user_repo)]
    [TestCase(_package_name + "@git@github.com:" + _user_repo + ".git", ExpectedResult = _user_repo)]
    [TestCase(_package_name + "@git@github.com:" + _user_repo + ".git#0.3.0", ExpectedResult = _user_repo)]
    [TestCase(_package_name + "@git:git@github.com:" + _user_repo + ".git", ExpectedResult = _user_repo)]
    [TestCase(_package_name + "@git:git@github.com:" + _user_repo + ".git#0.3.0",
        ExpectedResult = _user_repo)]
    public string GetRepoIdTest(string package_id) { return GithubExtension.GetRepoUrl(package_id); }

    [TestCase("", ExpectedResult = true)]
    [TestCase("true", ExpectedResult = true)]
    [TestCase("false", ExpectedResult = false)]
    [TestCase("false,true", ExpectedResult = true)]
    [TestCase("true,false", ExpectedResult = false)]
    public bool ElementVisibleTest(string operations) {
      var element = new VisualElement();
      if (0 < operations.Length) {
        foreach (var flag in operations.Split(',').Select(System.Convert.ToBoolean)) {
          GithubExtension.SetElementDisplay(element, flag);
        }
      }

      return GithubExtension.IsElementDisplay(element);
    }

    [TestCase("", ExpectedResult = false)]
    [TestCase("true", ExpectedResult = true)]
    [TestCase("false", ExpectedResult = false)]
    [TestCase("false,true", ExpectedResult = true)]
    [TestCase("true,false", ExpectedResult = false)]
    public bool ElementClassTest(string operations) {
      var element = new VisualElement();
      if (0 < operations.Length) {
        foreach (var flag in operations.Split(',').Select(System.Convert.ToBoolean)) {
          GithubExtension.SetElementClass(element, "test", flag);
        }
      }

      return GithubExtension.HasElementClass(element, "test");
    }

    [TestCase(LogType.Log,
        "Success",
        "https://api.github.com/repos/" + _user_repo + "/tags",
        ExpectedResult = null)]
    [TestCase(LogType.Error,
        "HTTP/1.1 404 Not Found",
        "https://api.github.com/repos/" + _user_repo + "/tags2",
        ExpectedResult = null)]
    [TestCase(LogType.Error,
        "Cannot resolve destination host",
        "https://api.githuberror.com/repos/" + _user_repo + "/tags",
        ExpectedResult = null)]
    [UnityTest]
    [Order(0)]
    public IEnumerator RequestTest(LogType log_type, string message, string url) {
      var path = GithubExtension.GetRequestCachePath(url);
      Debug.Log(path);
      if (File.Exists(path)) {
        File.Delete(path);
      }

      LogAssert.Expect(log_type, message);
      yield return GithubExtension.Request(url, x => Debug.Log("Success"));
    }

    [TestCase(LogType.Log,
        "Success",
        "https://api.github.com/repos/" + _user_repo + "/tags",
        ExpectedResult = null)]
    [UnityTest]
    [Order(1)]
    public IEnumerator RequestCacheTest(LogType log_type, string message, string url) {
      Assert.IsNotNull(GithubExtension.GetRequestCache(url));

      LogAssert.Expect(log_type, message);
      GithubExtension.Request(url, x => Debug.Log("Success"));
      yield break;
    }
  }
}
#endif
