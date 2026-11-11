using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;
using PapayAgenda.Domain.Entities;
using PapayAgenda.Infrastructure.Data;

namespace PapayAgenda.Application.Services
{
    public class EstabelecimentoService : IEstabelecimentoService
    {
        private readonly AppDbContext _context;

        public EstabelecimentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EstabelecimentoDto> CriarAsync(CriarEstabelecimentoDto dto)
        {
            var slugBase = string.IsNullOrWhiteSpace(dto.Slug) ? dto.Nome : dto.Slug;
            var slugTratado = GerarSlugValido(slugBase);

            var slugExistente = await _context.Estabelecimentos
                .AnyAsync(e => e.Slug == slugTratado);

            if (slugExistente)
            {
                throw new InvalidOperationException($"O slug '{slugTratado}' já está em uso por outro estabelecimento.");
            }

            var estabelecimento = new Estabelecimento
            {
                Nome = dto.Nome,
                Slug = slugTratado,
                TelefoneWhatsApp = dto.TelefoneWhatsApp,
                EmailGestor = dto.EmailGestor,
                CriadoEm = DateTime.UtcNow
            };

            _context.Estabelecimentos.Add(estabelecimento);
            await _context.SaveChangesAsync();

            return new EstabelecimentoDto
            {
                Id = estabelecimento.Id,
                Nome = estabelecimento.Nome,
                Slug = estabelecimento.Slug,
                TelefoneWhatsApp = estabelecimento.TelefoneWhatsApp,
                EmailGestor = estabelecimento.EmailGestor,
                Ativo = estabelecimento.Ativo,
                CriadoEm = estabelecimento.CriadoEm
            };
        }

        public async Task<IEnumerable<EstabelecimentoDto>> ObterTodosAsync()
        {
            return await _context.Estabelecimentos
                .Select(e => new EstabelecimentoDto
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Slug = e.Slug,
                    TelefoneWhatsApp = e.TelefoneWhatsApp,
                    EmailGestor = e.EmailGestor,
                    CriadoEm = e.CriadoEm
                })
                .ToListAsync();
        }

        public async Task<EstabelecimentoDto?> ObterPorSlugAsync(string slug)
        {
            var e = await _context.Estabelecimentos
                .FirstOrDefaultAsync(x => x.Slug == slug);

            if (e == null) return null;

            return new EstabelecimentoDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Slug = e.Slug,
                TelefoneWhatsApp = e.TelefoneWhatsApp,
                EmailGestor = e.EmailGestor,
                CriadoEm = e.CriadoEm
            };
        }

        private static string GerarSlugValido(string input)
        {
            var str = input.ToLowerInvariant();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", "-").Trim('-');
            return str;
        }
    }
}