using Microsoft.EntityFrameworkCore;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;
using PapayAgenda.Domain.Entities;
using PapayAgenda.Infrastructure.Data;

namespace PapayAgenda.Application.Services
{
    public class CategoriaServicoService : ICategoriaServicoService
    {
        private readonly AppDbContext _context;

        public CategoriaServicoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CategoriaServicoDto> CriarAsync(CriarCategoriaServicoDto dto)
        {
            var estabelecimentoExiste = await _context.Estabelecimentos
                .AnyAsync(e => e.Id == dto.EstabelecimentoId);

            if (!estabelecimentoExiste)
            {
                throw new InvalidOperationException($"O estabelecimento com ID {dto.EstabelecimentoId} não foi encontrado.");
            }

            var categoria = new CategoriaServico
            {
                Nome = dto.Nome,
                EstabelecimentoId = dto.EstabelecimentoId
            };

            _context.CategoriasServico.Add(categoria);
            await _context.SaveChangesAsync();

            return new CategoriaServicoDto
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                EstabelecimentoId = categoria.EstabelecimentoId
            };
        }

        public async Task<IEnumerable<CategoriaServicoDto>> ObterPorEstabelecimentoAsync(int estabelecimentoId)
        {
            return await _context.CategoriasServico
                .Where(c => c.EstabelecimentoId == estabelecimentoId)
                .Select(c => new CategoriaServicoDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    EstabelecimentoId = c.EstabelecimentoId
                })
                .ToListAsync();
        }

        public async Task<CategoriaServicoDto?> ObterPorIdAsync(int id)
        {
            var c = await _context.CategoriasServico.FindAsync(id);
            if (c == null) return null;

            return new CategoriaServicoDto
            {
                Id = c.Id,
                Nome = c.Nome,
                EstabelecimentoId = c.EstabelecimentoId
            };
        }
    }
}