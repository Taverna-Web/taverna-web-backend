using Application.Dtos.Cadastro.Command;
using Application.Dtos.Cadastro.Request;
using Riok.Mapperly.Abstractions;

namespace Application.Mapper.Cadastro;

[Mapper]
public static partial class CadastroMapper
{
    public static partial CadastroCommandDto ToCommand(this CadastroRequestDto request);

}
