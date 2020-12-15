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
namespace Xbim.Ifc2x3.PresentationDefinitionResource
{
	public partial class IfcAnnotationCurveOccurrence : IExpressValidatable
	{
		public enum IfcAnnotationCurveOccurrenceClause
		{
			WR31,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcAnnotationCurveOccurrenceClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcAnnotationCurveOccurrenceClause.WR31:
						retVal = !(Functions.EXISTS(this/* as IfcStyledItem*/.Item)) || (Functions.TYPEOF(this/* as IfcStyledItem*/.Item).Contains("IFC2X3.IFCCURVE"));
						break;
				}
			} catch (Exception ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc2x3.PresentationDefinitionResource.IfcAnnotationCurveOccurrence>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcAnnotationCurveOccurrence.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public override IEnumerable<ValidationResult> Validate()
		{
			foreach (var value in base.Validate())
			{
				yield return value;
			}
			if (!ValidateClause(IfcAnnotationCurveOccurrenceClause.WR31))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcAnnotationCurveOccurrence.WR31", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
