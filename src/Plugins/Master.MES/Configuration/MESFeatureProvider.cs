using Abp.Application.Features;
using Abp.Localization;
using Abp.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public class MESFeatureNames
    {
        public const string MESSupplier = "MESSupplier";
        public const string MESManufacture = "MESManufacture";
        public const string MESCustomer = "MESCustomer";
    }
    public class MESFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            context.Create(
             MESFeatureNames.MESManufacture,
             defaultValue: "false",
             displayName: L("发包方"),
             inputType: new CheckboxInputType()
             );
            context.Create(
             MESFeatureNames.MESSupplier,
             defaultValue: "false",
             displayName: L("接包方"),
             inputType: new CheckboxInputType()
             );
            context.Create(
             MESFeatureNames.MESCustomer,
             defaultValue: "false",
             displayName: L("客户"),
             inputType: new CheckboxInputType()
             );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MasterConsts.LocalizationSourceName);
        }
    }
}
