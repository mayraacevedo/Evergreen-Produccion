using EG.Models.Entities;
using EG.Models.Util;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace EG.DAL.Repositories
{
    public class SembradoRepository : Repository<Sembrado, EGDbContext>
    {

        private readonly EGDbContext _context;

        public SembradoRepository(EGDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedQueryResult> Get(
         int pageSize,
         int pageNumber,
         NameValueCollection? where = null)
        {
            PagedQueryResult result = new PagedQueryResult();

            var q = (from s in _context.Sembrados
                     orderby s.FechaCreacion descending
                     select new Sembrado
                     {
                         Id = s.Id,
                         Codigo = s.Codigo,
                         IdEstado = s.IdEstado,
                         Observaciones = s.Observaciones,
                         FechaCreacion = s.FechaCreacion,
                         IdEstadoNavigation = _context.EstadosSembrados.Where(x => x.Id.Equals(s.IdEstado)).FirstOrDefault(),

                     });

            q = q.OrderBy("FechaCreacion desc");

            if (where != null && where.Count > 0)
            {
                // Create a config object
                var config = new ParsingConfig
                {
                    UseParameterizedNamesInDynamicQuery = true
                };


                foreach (var field in where.AllKeys)
                {
                    switch (field)
                    {
                        case "IdParcela":
                            var data = Convert.ToInt32(where[field]);
                            var d = (from de in _context.SembradosDet
                                     where de.IdParcela.Equals(data)
                                     select de.IdSembrado).ToList();

                            q = q.Where(x => d.Contains(x.Id));
                                
                            break;
                        case "IdSemilla":
                            var dataS = Convert.ToInt32(where[field]);
                            var dS = (from de in _context.SembradosDet
                                     where de.IdSemilla.Equals(dataS)
                                     select de.IdSembrado).ToList();
                            
                            q = q.Where(x => dS.Contains(x.Id));
                            break;

                        default:
                            q = q.Where(config, $"{field} = @0", int.Parse(where[field]));
                            break;
                    }

                }
            }

            var totalRecords = q.Count();


            result.TotalRecords = totalRecords;
            result.CurrentPage = pageNumber;
            result.Data = await q.Skip(pageSize * pageNumber).Take(pageSize).Select(x => x as object).ToListAsync();

            return result;

        }
        public async Task<Sembrado> GetDetail(int Id)
        {
            var se = (from s in _context.Sembrados
                      join e in _context.EstadosSembrados on s.IdEstado equals e.Id
                      where s.Id.Equals(Id)
                      select new Sembrado
                      {
                          Id = s.Id,
                          Codigo = s.Codigo,
                          IdEstado = s.IdEstado,
                          NombreEstado = e.Nombre,
                          Observaciones = s.Observaciones,
                          FechaCreacion = s.FechaCreacion
                      }).FirstOrDefault();


            if (se.Id > 0)
            {
                se.SembradosDets = (from sd in _context.SembradosDet
                                    join e in _context.EstadosSembradosParcelas on sd.IdEstado equals e.Id
                                    join p in _context.Parcelas on sd.IdParcela equals p.Id
                                    join s in _context.Semillas on sd.IdSemilla equals s.Id
                                    where sd.IdSembrado.Equals(se.Id)
                                    select new SembradoDet
                                    {
                                        Id = sd.Id,
                                        IdSembrado = sd.IdSembrado,
                                        IdParcela = sd.IdParcela,
                                        IdSemilla = sd.IdSemilla,
                                        IdEstado = sd.IdEstado,
                                        Observaciones = sd.Observaciones,
                                        FechaCreacion = sd.FechaCreacion,
                                        NombreParcela = p.Nombre,
                                        NombreSemilla = s.Nombre,
                                        NombreEstado = e.Nombre
                                    }).ToList();
            }

            return se;
        }

        public override async Task<Sembrado> Add(Sembrado Item)
        {
            var tipoDocumento = _context.Consecutivos.Find(1);
            if (tipoDocumento != null)
            {

                tipoDocumento.Consecutivo = tipoDocumento.Consecutivo + 1;
                _context.Update(tipoDocumento);
                Item.Codigo = $"{tipoDocumento.Prefijo}-{tipoDocumento.Consecutivo:D6}";

                Item.IdEstadoNavigation = null;

                foreach (var row in Item.SembradosDets)
                {
                    row.IdEstadoNavigation = null;
                    row.IdParcelaNavigation = null;
                    row.IdSemillaNavigation = null;
                }

                _context.Add(Item);
                await _context.SaveChangesAsync();
                return Item;
            }
            else
                throw new Exception("Tipo de consecutivo no encontrado");
        }
        public async Task<bool> Delete(int Id)
        {
            var sem = _context.Sembrados.Where(x => x.Id.Equals(Id)).Include(x => x.SembradosDets).FirstOrDefault();

            if (sem != null)
            {
                _context.Sembrados.Remove(sem);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        public async Task<List<Sembrado>> GetExcelInfo(
         int pageSize,
         int pageNumber,
         NameValueCollection? where = null)
        {

            var q = (from s in _context.Sembrados
                     orderby s.FechaCreacion descending
                     select new Sembrado
                     {
                         Id = s.Id,
                         Codigo = s.Codigo,
                         IdEstado = s.IdEstado,
                         Observaciones = s.Observaciones,
                         FechaCreacion = s.FechaCreacion,
                         IdEstadoNavigation = _context.EstadosSembrados.Where(x => x.Id.Equals(s.IdEstado)).FirstOrDefault(),
                         SembradosDets = _context.SembradosDet.Include(x=> x.IdParcelaNavigation).Include(x=> x.IdEstadoNavigation).Include(x=> x.IdSemillaNavigation).Where(x => x.IdSembrado.Equals(s.Id)).ToList()

                     });

            if (where != null && where.Count > 0)
            {
                // Create a config object
                var config = new ParsingConfig
                {
                    UseParameterizedNamesInDynamicQuery = true
                };


                foreach (var field in where.AllKeys)
                {
                    switch (field)
                    {
                        case "IdParcela":
                            var data = Convert.ToInt32(where[field]);
                            var d = (from de in _context.SembradosDet
                                     where de.IdParcela.Equals(data)
                                     select de.IdSembrado).ToList();

                            q = q.Where(x => d.Contains(x.Id));

                            break;
                        case "IdSemilla":
                            var dataS = Convert.ToInt32(where[field]);
                            var dS = (from de in _context.SembradosDet
                                      where de.IdSemilla.Equals(dataS)
                                      select de.IdSembrado).ToList();

                            q = q.Where(x => dS.Contains(x.Id));
                            break;

                        default:
                            q = q.Where(config, $"{field} = @0", int.Parse(where[field]));
                            break;
                    }

                }
            }

           
            return q.ToList();

        }
    }

}
