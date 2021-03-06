﻿#region License
// The PostgreSQL License
//
// Copyright (C) 2017 The Npgsql Development Team
//
// Permission to use, copy, modify, and distribute this software and its
// documentation for any purpose, without fee, and without a written
// agreement is hereby granted, provided that the above copyright notice
// and this paragraph and the following two paragraphs appear in all copies.
//
// IN NO EVENT SHALL THE NPGSQL DEVELOPMENT TEAM BE LIABLE TO ANY PARTY
// FOR DIRECT, INDIRECT, SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES,
// INCLUDING LOST PROFITS, ARISING OUT OF THE USE OF THIS SOFTWARE AND ITS
// DOCUMENTATION, EVEN IF THE NPGSQL DEVELOPMENT TEAM HAS BEEN ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.
//
// THE NPGSQL DEVELOPMENT TEAM SPECIFICALLY DISCLAIMS ANY WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
// AND FITNESS FOR A PARTICULAR PURPOSE. THE SOFTWARE PROVIDED HEREUNDER IS
// ON AN "AS IS" BASIS, AND THE NPGSQL DEVELOPMENT TEAM HAS NO OBLIGATIONS
// TO PROVIDE MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS, OR MODIFICATIONS.
#endregion

using JetBrains.Annotations;
using Npgsql.BackendMessages;
using Npgsql.PostgresTypes;
using Npgsql.TypeHandling;
using Npgsql.TypeMapping;
using NpgsqlTypes;

namespace Npgsql.TypeHandlers.GeometricHandlers
{
    /// <summary>
    /// Type handler for the PostgreSQL geometric line segment type.
    /// </summary>
    /// <remarks>
    /// http://www.postgresql.org/docs/current/static/datatype-geometric.html
    /// </remarks>
    [TypeMapping("lseg", NpgsqlDbType.LSeg, typeof(NpgsqlLSeg))]
    class LineSegmentHandler : NpgsqlSimpleTypeHandler<NpgsqlLSeg>, INpgsqlSimpleTypeHandler<string>
    {
        public override NpgsqlLSeg Read(NpgsqlReadBuffer buf, int len, FieldDescription fieldDescription = null)
            => new NpgsqlLSeg(buf.ReadDouble(), buf.ReadDouble(), buf.ReadDouble(), buf.ReadDouble());

        string INpgsqlSimpleTypeHandler<string>.Read(NpgsqlReadBuffer buf, int len, [CanBeNull] FieldDescription fieldDescription)
            => Read(buf, len, fieldDescription).ToString();

        protected override int ValidateAndGetLength(object value, NpgsqlParameter parameter = null)
        {
            if (!(value is NpgsqlLSeg))
                throw CreateConversionException(value.GetType());
            return 32;
        }

        protected override void Write(object value, NpgsqlWriteBuffer buf, NpgsqlParameter parameter = null)
        {
            var v = (NpgsqlLSeg)value;
            buf.WriteDouble(v.Start.X);
            buf.WriteDouble(v.Start.Y);
            buf.WriteDouble(v.End.X);
            buf.WriteDouble(v.End.Y);
        }
    }
}
