using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Siils.Api.Models;

namespace SLYEx.API.Dummy
{
    public class SampleRepository : BaseRepository
    {
        public List<Price> GetPrices(DateTime date)
        {
            const string sql = @"
        SELECT [TimeStamp]
              ,[Source]
              ,[Target]
              ,[Open]
              ,[Close]
              ,[High]
              ,[Low]
          FROM [dbo].[SAMPLE] WITH(NOLOCK)
          WHERE [TimeStamp] = @TimeStamp
          ORDER BY [Id] ASC
";
            var result = new List<Price>();

            try
            {
                using (var connection = OpenConnection())
                {
                    result = connection.Query<Price>(sql, new
                    {
                        TimeStamp = date
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}