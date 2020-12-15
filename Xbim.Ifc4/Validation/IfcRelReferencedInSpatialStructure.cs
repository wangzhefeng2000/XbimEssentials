using System;
using Microsoft.Extensions.Logging;
using Xbim.Common;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Xbim.Common.Enumerations;
using Xbim.Common.ExpressValidation;
using Xbim.Ifc4.Interfaces;
// ReSharper disable once CheckNamespace
// ReSharper disable InconsistentNaming
namespace Xbim.Ifc4.ProductExtension
{
	public partial class IfcRelReferencedInSpatialStructure : IExpressValidatable
	{
		public enum IfcRelReferencedInSpatialStructureClause
		{
			WR31,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcRelReferencedInSpatialStructureClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcRelReferencedInSpatialStructureClause.WR31:
						retVal = Functions.SIZEOF(RelatedElements.Where(temp => Functions.TYPEOF(temp).Contains("IFC4.IFCSPATIALSTRUCTUREELEMENT"))) == 0;
						break;
				}
			} catch (Exception  ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc4.ProductExtension.IfcRelReferencedInSpatialStructure>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcRelReferencedInSpatialStructure.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public virtual IEnumerable<ValidationResult> Validate()
		{
			if (!ValidateClause(IfcRelReferencedInSpatialStructureClause.WR31))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcRelReferencedInSpatialStructure.WR31", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
