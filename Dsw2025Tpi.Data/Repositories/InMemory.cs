using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Data.Repositories
{
    public class InMemory: IRepository
    {
        private List<Product>? _products;
        public InMemory()
        {
            loadProduct();
        }

        private void loadProduct()
        {
            var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "C:\\Users\\moran\\OneDrive\\Desktop\\DSW2025\\tfi\\Dsw2025Tpi\\Dsw2025Tpi.Data\\Sources\\products.json"));
            _products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }

        public async Task<T> Add<T>(T entity) where T : EntityBase
        {
            var set = await GetSet<T>();
            set.Add(entity);

            if (typeof(T) == typeof(Product))
            {
                var filePath = "C:\\Users\\moran\\OneDrive\\Desktop\\DSW2025\\tfi\\Dsw2025Tpi\\Dsw2025Tpi.Data\\Sources\\products.json";
                
                File.WriteAllText(filePath, JsonSerializer.Serialize(set, new JsonSerializerOptions { WriteIndented = true }));
            }

            return await Task.FromResult(entity);
        }

        public async Task<T> Delete<T>(T entity) where T : EntityBase
        {
            var set = await GetSet<T>();
            return await Task.FromResult(set.Remove(entity) ? entity : null);
        }

        public async Task<T?> First<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
        {
            var set = await GetSet<T>();
            return await Task.FromResult(set?.AsQueryable().FirstOrDefault(predicate));
        }

        public async Task<IEnumerable<T>?> GetAll<T>(params string[] include) where T : EntityBase
        {
            var set = await GetSet<T>();
            return set?.ToList();
        }

        public async Task<T?> GetById<T>(Guid id, params string[] include) where T : EntityBase
        {
            var set = await GetSet<T>();
            return set?.FirstOrDefault(e => e.Id == id);
        }

        

        private async Task<List<T>?> GetSet<T>() where T : EntityBase
        {
            if (typeof(T) == typeof(Product))
            {
                return _products as List<T>;
            }
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>?> GetFiltered<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
        {
            var set = await GetSet<T>();
            return await Task.FromResult(set?.AsQueryable().Where(predicate).ToList());
        }

        public async Task<T> Update<T>(T entity) where T : EntityBase
        {
            var set = await GetSet<T>();
            var productos = await Task.FromResult(set?.Select(e => e.Id == entity.Id ? entity : e).ToList());
            // sobrescribír el JSON
            string rutaArchivo = "C:\\Users\\moran\\OneDrive\\Desktop\\DSW2025\\tfi\\Dsw2025Tpi\\Dsw2025Tpi.Data\\Sources\\products.json";
            string json = JsonSerializer.Serialize(productos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(rutaArchivo, json);

            return await Task.FromResult(set?.FirstOrDefault(e => e.Id == entity.Id) ?? entity);
        }

        
    }
}
