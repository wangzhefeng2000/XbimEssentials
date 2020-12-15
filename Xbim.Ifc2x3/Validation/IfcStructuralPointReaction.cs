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
namespace Xbim.Ifc2x3.StructuralAnalysisDomain
{
	public partial class IfcStructuralPointReaction : IExpressValidatable
	{
		public enum IfcStructuralPointReactionClause
		{
			WR61,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcStructuralPointReactionClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcStructuralPointReactionClause.WR61:
						retVal = Functions.SIZEOF(Functions.NewArray("IFC2X3.IFCSTRUCTURALLOADSINGLEFORCE", "IFC2X3.IFCSTRUCTURALLOADSINGLEDISPLACEMENT") * Functions.TYPEOF(this/* as IfcStructuralActivity*/.AppliedLoad)) == 1;
						break;
				}
			} catch (Exception ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc2x3.StructuralAnalysisDomain.IfcStructuralPointReaction>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcStructuralPointReaction.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public override IEnumerable<ValidationResult> Validate()
		{
			foreach (var value in base.Validate())
			{
				yield return value;
			}
			if (!ValidateClause(IfcStructuralPointReactionClause.WR61))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcStructuralPointReaction.WR61", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
