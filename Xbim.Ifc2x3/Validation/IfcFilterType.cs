using System;
using Microsoft.Extensions.Logging;
using Xbim.Common;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Xbim.Common.Enumerations;
using Xbim.Common.ExpressValidation;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.Interfaces;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProfileResource;
using Xbim.Ifc2x3.ProfilePropertyResource;
// ReSharper disable once CheckNamespace
// ReSharper disable InconsistentNaming
namespace Xbim.Ifc2x3.HVACDomain
{
	public partial class IfcFilterType : IExpressValidatable
	{
		public enum IfcFilterTypeClause
		{
			WR1,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcFilterTypeClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcFilterTypeClause.WR1:
						retVal = (PredefinedType != IfcFilterTypeEnum.USERDEFINED) || ((PredefinedType == IfcFilterTypeEnum.USERDEFINED) && Functions.EXISTS(this/* as IfcElementType*/.ElementType));
						break;
				}
			} catch (Exception ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc2x3.HVACDomain.IfcFilterType>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcFilterType.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public override IEnumerable<ValidationResult> Validate()
		{
			foreach (var value in base.Validate())
			{
				yield return value;
			}
			if (!ValidateClause(IfcFilterTypeClause.WR1))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcFilterType.WR1", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
