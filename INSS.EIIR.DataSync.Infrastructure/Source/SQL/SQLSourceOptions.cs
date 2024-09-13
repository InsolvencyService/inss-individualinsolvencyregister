using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Source.SQL
{
    public class SQLSourceOptions
    {
        private readonly IMapper _mapper;
        private readonly string? _connectionString;

        public SQLSourceOptions(IMapper mapper, string? connectionString)
        {
            _mapper = mapper;
            _connectionString = connectionString;
        }

        public string? ConnectionString { get => _connectionString; }
        public IMapper Mapper { get => _mapper; }
    }
}
