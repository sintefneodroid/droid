using Boo.Lang;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using UnityEngine;
using Random = System.Random;

namespace droid.Runtime.Prototyping.Configurables
{
  [AddComponentMenu(
    ConfigurableComponentMenuPath._ComponentMenuPath
    + "ChildSelector"
    + ConfigurableComponentMenuPath._Postfix)]
  public class ChildSelectorConfigurable : Configurable, ICategoryProvider
  {
    [SerializeField] GameObject active;
    [SerializeField] GameObject[] children;
    [SerializeField] int len;

    public override void PostEnvironmentSetup()
    {
      var la = new List<GameObject>();
      foreach (Transform child in this.transform)
      {
        var o = child.gameObject;
        o.SetActive(false);
        this.active = o;
        la.Add(o);
      }

      this.children = la.ToArray();

      this.len = this.transform.childCount;

      this.active.SetActive(true);
    }

    public override void ApplyConfiguration(IConfigurableConfiguration configuration)
    {
      if(this.active)
      {
        this.active.SetActive(false);
      }
      if(this.children!=null && (int)configuration.ConfigurableValue<this.len)
      {
        CurrentCategoryValue = (int) configuration.ConfigurableValue;
        this.active = this.children[CurrentCategoryValue];
      }

      this.active.SetActive(true);
    }

    public override IConfigurableConfiguration SampleConfiguration(Random random_generator)
    {

      return new Configuration(this.Identifier, random_generator.Next(0,this.len));
    }

    public int CurrentCategoryValue { get; set; }
  }
}