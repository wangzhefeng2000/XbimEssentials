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
namespace Xbim.Ifc2x3.CostResource
{
	public partial class IfcAppliedValue : IExpressValidatable
	{
		public enum IfcAppliedValueClause
		{
			WR1,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcAppliedValueClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcAppliedValueClause.WR1:
						retVal = Functions.EXISTS(AppliedValue) || Functions.EXISTS(ValueOfComponents);
						break;
				}
			} catch (Exception ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc2x3.CostResource.IfcAppliedValue>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcAppliedValue.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public virtual IEnumerable<ValidationResult> Validate()
		{
			if (!ValidateClause(IfcAppliedValueClause.WR1))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcAppliedValue.WR1", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
