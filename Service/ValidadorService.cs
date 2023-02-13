using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistirArquirvos.Service
{
    public class ValidadorService
    {
        public ValidadorService()
        {

        }
        public void ValidarArquivos(IEnumerable<string> lines)
        {
            var exceptions = new BlockingCollection<Exception>();
            Parallel.ForEach(lines, line =>
            {
                try
                {
                    var columns = line.Split('\t');
                    if (columns.Length != 9)
                    {
                        exceptions.Add(new Exception("Formato de arquivo invalido: " + line));
                    }
                    var titulo = columns[2].Trim();
                    if (string.IsNullOrWhiteSpace(titulo))
                    {
                        exceptions.Add(new Exception("Formato de titulo invalido: " + titulo));
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
                if (exceptions.Count > 0)
                {
                    throw new AggregateException("Formato invalido. Use um arquivo compativel.", exceptions.ToList());
                }
            });
        }
    }
}
