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
namespace Xbim.Ifc2x3.GeometryResource
{
	public partial class IfcCartesianTransformationOperator3DnonUniform : IExpressValidatable
	{
		public enum IfcCartesianTransformationOperator3DnonUniformClause
		{
			WR1,
			WR2,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcCartesianTransformationOperator3DnonUniformClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcCartesianTransformationOperator3DnonUniformClause.WR1:
						retVal = Scl2 > 0;
						break;
					case IfcCartesianTransformationOperator3DnonUniformClause.WR2:
						retVal = Scl3 > 0;
						break;
				}
			} catch (Exception ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc2x3.GeometryResource.IfcCartesianTransformationOperator3DnonUniform>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcCartesianTransformationOperator3DnonUniform.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public override IEnumerable<ValidationResult> Validate()
		{
			foreach (var value in base.Validate())
			{
				yield return value;
			}
			if (!ValidateClause(IfcCartesianTransformationOperator3DnonUniformClause.WR1))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcCartesianTransformationOperator3DnonUniform.WR1", IssueType = ValidationFlags.EntityWhereClauses };
			if (!ValidateClause(IfcCartesianTransformationOperator3DnonUniformClause.WR2))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcCartesianTransformationOperator3DnonUniform.WR2", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
