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

            var servico = new Servico
            {
                Nome = dto.Nome,
                Preco = dto.Preco,
                DuracaoMinutos = dto.DuracaoMinutos,
                OrdemExibicao = dto.OrdemExibicao > 0 ? dto.OrdemExibicao : 1,
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
                OrdemExibicao = servico.OrdemExibicao,
                EstabelecimentoId = servico.EstabelecimentoId
            };
        }

        public async Task<IEnumerable<ServicoDto>> ObterPorEstabelecimentoAsync(int estabelecimentoId)
        {
            return await _context.Servicos
                .Where(s => s.EstabelecimentoId == estabelecimentoId)
                .OrderBy(s => s.OrdemExibicao)
                .ThenBy(s => s.Id)
                .Select(s => new ServicoDto
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Preco = s.Preco,
                    DuracaoMinutos = s.DuracaoMinutos,
                    Ativo = s.Ativo,
                    OrdemExibicao = s.OrdemExibicao,
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
                OrdemExibicao = s.OrdemExibicao,
                EstabelecimentoId = s.EstabelecimentoId
            };
        }
    }
}