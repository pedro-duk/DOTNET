## UseHttpsRedirection versus RequireHttps

- RequireHttps: atributo aplicado individualmente a controllers ou views que faz o mesmo redirecionamento. Em APIs **não é recomendado**, pois misturar http com https abre o risco de ataques de HTTPS downgrade.

- app.UseHttpsRedirection: Aplica middleware de redirecionamento http -> https em todas as requisições enviadas.

  - Porém usar só esse não é suficiente pra previnir os ataques de downgrade. Para garantir, use também o app.UseHsts() - Header Strict-Transport-Security que garante que todo recurso só será acessado via https.

## Serialização versus Desserialização
- Serialização: Converter objetos em Strings JSON:
```JsonSerializer.Serialize(Pessoa)```
- Desserialização: Strings JSON em objetos:
```JsonSerializer.Deserialize<Pessoa>(json)```

## Injeção de Dependência
Padrão usado para implementar a Inversão de Controle (IoC), reduzindo o acoplamento entre os objetos. Tornar uma classe independente das suas dependências.

- Configurar a injeção de dependência no Contêiner DI no .NET Core: permite que implementações sejam injetadas nas interfaces!

```services.AddTransient<IPedido, Pedido>()```
Conteinei DI sabe que, a cada vez que usar IPedido, deve resolver uma instância Pedido.

- Princípios
  - Inversão de Controle
  - Princípio de Inversão da Dependência

- Padrões
  - Injeção de Dependência

- Frameworks
  - Container de Injeção de Dependência: .NET possui um nativo!

.NET Suporta padrão da injeção de dependência, que é uma técnica para obter inversão de controle usando o container nativo DI (IoC Container)

## Lifetimes Service - Tempo de Vida útil do serviço
Ao registrar serviços em um container, é importante definir o tempo de vida útil que queremos usar para o serviço. Isso controla por quanto tempo um objeto vai existir após ter sido criado pelo container.
- Pode impactar a performance da aplicação (uso de memória ou afetar estado dos objetos)
- Usar o Método de Extensão apropriado no IServiceCollection (para o container DI Nativo) ao registrar o serviço.

Serviços podem ser registrados como:
- Transient: criados cada vez que são solicitados. Indicado para serviços leves e sem estados. **AddTransient**
- Scoped: criados em cada solicitação (uma vez por solicitação/request do cliente). Indicado para aplicações WEB. Se durante um request você usar a mesma injeção de dependência em muitos lugares, é usada a mesma instância do objeto. **AddScoped**
- Singleton: Criados uma vez durante a vida útil do aplicativo. Usada a mesma instância para todo o aplicativo. Para serviços thread-safe e geralmente sem estado. **AddSingleton**

Container de DI (Dependency injection) mantém referências à todos os serviços criados, e libera para o garbage collector após o término da vida útil deles.
 






## Outros
- Níveis de Maturidade de Richardson: classifica APIs com base na aderência e conformidade com o modelo REST. Nível 2 não implementa todas mas ainda é considerada Restful.

## Links úteis
- https://jsoneditoronline.org/
- https://json2csharp.com/