using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;
  public class ButtonPlayNow : MonoBehaviour
  {
    public void PlayNowButton(string sceneName)
    {
      Application.targetFrameRate = 60;

      var anim = gameObject.GetComponentsInChildren<AnimHierarchy>(false);

      float time = 1;

      foreach(var a in anim)
      {
        time = Mathf.Max(time,a.DoAnimOut());
      }
      DOVirtual.DelayedCall(time,() => {
        //This currently launches into the "Game" scene.
        //Launch Skillz Entry point here
        //SceneManager.LoadSceneAsync(sceneName);
        SkillzCrossPlatform.LaunchSkillz();
			});
    }
  }
