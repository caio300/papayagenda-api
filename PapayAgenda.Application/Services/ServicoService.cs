using Microsoft.EntityFrameworkCore;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;
using PapayAgenda.Domain.Entities;
using PapayAgenda.Infrastructure.Data;

namespace PapayAgenda.Application.Services
{
    public class ServicoService : IServicoService
    {
        private readonly AppDbContext _context;

        public ServicoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServicoDto> CriarAsync(CriarServicoDto dto)
        {        
            var estabelecimentoExiste = await _context.Estabelecimentos
                .AnyAsync(e => e.Id == dto.EstabelecimentoId);

            if (!estabelecimentoExiste)
            {
                throw new InvalidOperationException($"O estabelecimento com ID {dto.EstabelecimentoId} não foi encontrado.");
            }

            // Valida se a categoria existe e se pertence ao estabelecimento
            var categoriaExiste = await _context.CategoriasServico
                .AnyAsync(c => c.Id == dto.CategoriaServicoId && c.EstabelecimentoId == dto.EstabelecimentoId);

            if (!categoriaExiste)
            {
                throw new InvalidOperationException($"A categoria informada não existe ou não pertence a este estabelecimento.");
            }

            var servico = new Servico
            {
                Nome = dto.Nome,
                Preco = dto.Preco,
                DuracaoMinutos = dto.DuracaoMinutos,
                CategoriaServicoId = dto.CategoriaServicoId,
                EstabelecimentoId = dto.EstabelecimentoId,
                Ativo = true
            };

            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();

            return new ServicoDto
            {
                Id = servico.Id,
                Nome = servico.Nome,
                Preco = servico.Preco,
                DuracaoMinutos = servico.DuracaoMinutos,
                Ativo = servico.Ativo,
                CategoriaServicoId = servico.CategoriaServicoId,
                EstabelecimentoId = servico.EstabelecimentoId
            };
        }

        public async Task<IEnumerable<ServicoDto>> ObterPorEstabelecimentoAsync(int estabelecimentoId)
        {
            return await _context.Servicos
                .Where(s => s.EstabelecimentoId == estabelecimentoId)
                .Select(s => new ServicoDto
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Preco = s.Preco,
                    DuracaoMinutos = s.DuracaoMinutos,
                    Ativo = s.Ativo,
                    CategoriaServicoId = s.CategoriaServicoId,
                    EstabelecimentoId = s.EstabelecimentoId
                })
                .ToListAsync();
        }

        public async Task<ServicoDto?> ObterPorIdAsync(int id)
        {
            var s = await _context.Servicos.FindAsync(id);

            if (s == null) return null;

            return new ServicoDto
            {
                Id = s.Id,
                Nome = s.Nome,
                Preco = s.Preco,
                DuracaoMinutos = s.DuracaoMinutos,
                Ativo = s.Ativo,
                CategoriaServicoId = s.CategoriaServicoId,
                EstabelecimentoId = s.EstabelecimentoId
            };
        }
    }
}