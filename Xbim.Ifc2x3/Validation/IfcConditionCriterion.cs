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
namespace Xbim.Ifc2x3.FacilitiesMgmtDomain
{
	public partial class IfcConditionCriterion : IExpressValidatable
	{
		public enum IfcConditionCriterionClause
		{
			WR1,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcConditionCriterionClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcConditionCriterionClause.WR1:
						retVal = Functions.EXISTS(this/* as IfcRoot*/.Name);
						break;
				}
			} catch (Exception ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc2x3.FacilitiesMgmtDomain.IfcConditionCriterion>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcConditionCriterion.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public override IEnumerable<ValidationResult> Validate()
		{
			foreach (var value in base.Validate())
			{
				yield return value;
			}
			if (!ValidateClause(IfcConditionCriterionClause.WR1))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcConditionCriterion.WR1", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
