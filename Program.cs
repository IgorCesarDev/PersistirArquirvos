using PersistirArquirvos.Data;
using PersistirArquirvos.Models;
using PersistirArquirvos.Service;
using System;
using System.Collections.Concurrent;

class Program
{
    static void Main(string[] args)
    {
        var path = @"C:\Users\Igor\source\repos\PersistirArquirvos - Copia\data.tsv";
        var lines = File.ReadAllLines(path).Skip(1);
        try
        {
            ValidadorService validadorService = new ValidadorService();
            validadorService.ValidarArquivos(lines);
        }
        catch(AggregateException ex)
        {
            Console.WriteLine("Erro ao validar arquivo: "+ ex.Message);
        }
        try
        {
            using (var context = new AppDbContext())
            {
                FilmeService filmeService = new FilmeService(context);
                filmeService.AdicionarFilmes(lines);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro" + ex.Message);
        }
    }
}