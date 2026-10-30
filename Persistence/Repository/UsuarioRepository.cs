using Domain.Exceptions.Common;
using Domain.Interfaces.Repository;
using Domain.Model;
using Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repository;

public class UsuarioRepository(ApplicationDbContext context) : IUsuarioRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int> ObterIdUsuarioComEmailRepositoryAsync(Email enderecoEmail)
    {
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.EnderecoEmailUsuario == enderecoEmail)
            ?? throw new NotFoundException("Usuário não encontrado com o email fornecido.");
        if (usuario.ContaDeletada)
        {
            throw new NotFoundException("A conta associada a este email foi deletada.");
        }
        return usuario.IdUsuario;
    }

    public async Task<bool> EmailJaCadastradoRepositoryAsync(Email enderecoEmail)
    {
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .AnyAsync(u => u.EnderecoEmailUsuario == enderecoEmail);

        return usuario;
    }
    public async Task AdicionarUsuarioRepositoryAsync(string NomeUsuario, string Senha, Email enderecoEmail)
    {
        var novoUsuario = new Usuario
        {
            NomeUsuario = NomeUsuario,
            SenhaUsuario = Senha,
            EnderecoEmailUsuario = enderecoEmail,
        };
        _context.Usuarios.Add(novoUsuario);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarUsuarioRepositoryAsync(int IdUsuario)
    {
        var conta = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == IdUsuario)
            ?? throw new NotFoundException("Usuário não encontrado.");
        if (conta.ContaDeletada)
        {
            throw new NotFoundException("A conta associada a este usuário já foi deletada.");
        }
        conta.ContaDeletada = true;
        await _context.SaveChangesAsync();
    }

    public async Task<string> ObterEmailUsuarioRepositoryAsync(int IdUsuario)
    {
        var usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == IdUsuario)
            ?? throw new NotFoundException("Usuário não encontrado.");
        if (usuario.ContaDeletada)
        {
            throw new NotFoundException("A conta associada a este usuário foi deletada.");
        }
        return usuario.EnderecoEmailUsuario.EnderecoEmail;
    }

    public async Task<string> ObterNomeUsuarioRepositoryAsync(int IdUsuario)
    {
        var usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == IdUsuario)
            ?? throw new NotFoundException("Usuário não encontrado.");
        if (usuario.ContaDeletada)
        {
            throw new NotFoundException("A conta associada a este usuário foi deletada.");
        }
        return usuario.NomeUsuario;
    }

    public async Task<string> ObterSenhaUsuarioRepositoryAsync(int IdUsuario)
    {
        var usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == IdUsuario)
            ?? throw new NotFoundException("Usuário não encontrado.");
        if (usuario.ContaDeletada)
        {
            throw new NotFoundException("A conta associada a este usuário foi deletada.");
        }
        return usuario.SenhaUsuario;
    }

    public async Task RedefinirEmailUsuarioRepositoryAsync(int IdUsuario, Email enderecoEmail)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == IdUsuario)
            ?? throw new NotFoundException("Usuário não encontrado.");
        if (usuario.ContaDeletada)
        {
            throw new NotFoundException("A conta associada a este usuário foi deletada.");
        }
        usuario.EnderecoEmailUsuario = enderecoEmail;
        await _context.SaveChangesAsync();
    }

    public async Task RedefinirNomeUsuarioRepositoryAsync(int IdUsuario, string NomeUsuario)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == IdUsuario)
            ?? throw new NotFoundException("Usuário não encontrado.");
        if (usuario.ContaDeletada)
        {
            throw new NotFoundException("A conta associada a este usuário foi deletada.");
        }
        usuario.NomeUsuario = NomeUsuario;
        await _context.SaveChangesAsync();
    }

    public async Task RedefinirSenhaUsuarioRepositoryAsync(int IdUsuario, string Senha)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == IdUsuario)
            ?? throw new NotFoundException("Usuário não encontrado.");
        if (usuario.ContaDeletada)
        {
            throw new NotFoundException("A conta associada a este usuário foi deletada.");
        }
        usuario.SenhaUsuario = Senha;
        await _context.SaveChangesAsync();
    }
}
