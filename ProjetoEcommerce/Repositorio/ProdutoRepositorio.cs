using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;


namespace ProjetoEcommerce.Repositorio
{
    // Define a classe responsável por interagir com os dados de clientes no banco de dados
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        // Declara uma variável privada somente leitura para armazenar a string de conexão com o MySQL
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");


        // Método para cadastrar um novo cliente no banco de dados
        public void Cadastrar(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                // Cria um novo comando SQL para inserção de dados
                MySqlCommand cmd = new MySqlCommand("insert into Produto (NomeProd, DescProd, QuantProd, PrecoProd) values (@NomeProd, @DescProd, @QuantProd, @PrecoProd)", conexao);

                // Limpa os parâmetros do comando caso já existam
                cmd.Parameters.Clear();

                // Adiciona os parâmetros ao comando
                cmd.Parameters.Add("@NomeProd", MySqlDbType.VarChar).Value = produto.NomeProd;
                cmd.Parameters.Add("@DescProd", MySqlDbType.VarChar).Value = produto.DescProd;
                cmd.Parameters.Add("@QuantProd", MySqlDbType.Int32).Value = produto.QuantProd;
                cmd.Parameters.Add("@PrecoProd", MySqlDbType.Double).Value = produto.PrecoProd;

                // Executa o comando de inserção
                cmd.ExecuteNonQuery();

                conexao.Close();
            }


        }

        // Método para Editar (atualizar) os dados de um cliente existente no banco de dados
        public bool Atualizar(Produto produto)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("Update Produto set NomeProd=@nomeProd, DescProd=@Descricao, QuantProd=@Quantidade, PrecoProd=@Preco " + " where CodProd=@codigoProd ", conexao);
                    cmd.Parameters.Add("@codigoProd", MySqlDbType.Int32).Value = produto.CodProd;
                    cmd.Parameters.Add("@nomeProd", MySqlDbType.VarChar).Value = produto.NomeProd;
                    cmd.Parameters.Add("@Descricao", MySqlDbType.VarChar).Value = produto.DescProd;
                    cmd.Parameters.Add("@Quantidade", MySqlDbType.Int32).Value = produto.QuantProd;
                    cmd.Parameters.Add("@Preco", MySqlDbType.Double).Value = produto.PrecoProd;
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0; 

                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar Produto: {ex.Message}");
                return false;

            }
        }

        public IEnumerable<Produto> TodosProdutos()
        {
            // Cria uma nova lista para armazenar os objetos Cliente
            List<Produto> Produtolist = new List<Produto>();

            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para selecionar todos os registros da tabela 'cliente'
                MySqlCommand cmd = new MySqlCommand("SELECT * from Produto", conexao);

                // Cria um adaptador de dados para preencher um DataTable com os resultados da consulta
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                // Cria um novo DataTable
                DataTable dt = new DataTable();
                // metodo fill- Preenche o DataTable com os dados retornados pela consulta
                da.Fill(dt);
                // Fecha explicitamente a conexão com o banco de dados 
                conexao.Close();

                // interage sobre cada linha (DataRow) do DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    // Cria um novo objeto Cliente e preenche suas propriedades com os valores da linha atual
                    Produtolist.Add(
                                new Produto
                                {
                                    CodProd = Convert.ToInt32(dr["CodProd"]), // Converte o valor da coluna "codigo" para inteiro
                                    NomeProd = ((string)dr["NomeProd"]), // Converte o valor da coluna "nome" para string
                                    DescProd = ((string)dr["DescProd"]), // Converte o valor da coluna "telefone" para string
                                    QuantProd = Convert.ToInt32(dr["QuantProd"]),
                                    PrecoProd = Convert.ToDouble(dr["PrecoProd"]), // Converte o valor da coluna "email" para string
                                });
                }
                // Retorna a lista de todos os clientes
                return Produtolist;
            }
        }

        // Método para buscar um cliente específico pelo seu código (Codigo)
        public Produto ObterProduto(int CodigoProd)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para selecionar um registro da tabela 'cliente' com base no código
                MySqlCommand cmd = new MySqlCommand("SELECT * from Produto where CodProd=@codigoProd ", conexao);

                // Adiciona um parâmetro para o código a ser buscado, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@codigoProd", CodigoProd);

                // Cria um adaptador de dados (não utilizado diretamente para ExecuteReader)
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                // Declara um leitor de dados do MySQL
                MySqlDataReader dr;
                // Cria um novo objeto Cliente para armazenar os resultados
                Produto produto = new Produto();

                /* Executa o comando SQL e retorna um objeto MySqlDataReader para ler os resultados
                CommandBehavior.CloseConnection garante que a conexão seja fechada quando o DataReader for fechado*/

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                // Lê os resultados linha por linha
                while (dr.Read())
                {
                    // Preenche as propriedades do objeto Cliente com os valores da linha atual
                    produto.CodProd = Convert.ToInt32(dr["CodProd"]);//propriedade Codigo e convertendo para int
                    produto.NomeProd = (string)(dr["NomeProd"]); // propriedade Nome e passando string
                    produto.DescProd = (string)(dr["DescProd"]); //propriedade telefone e passando string
                    produto.QuantProd = Convert.ToInt32(dr["QuantProd"]);
                    produto.PrecoProd = Convert.ToDouble(dr["PrecoProd"]); //propriedade email e passando string
                }
                // Retorna o objeto Cliente encontrado (ou um objeto com valores padrão se não encontrado)
                return produto;
            }
        }


        // Método para excluir um cliente do banco de dados pelo seu código (ID)
        public void ExcluirProd(int Id)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();

                // Cria um novo comando SQL para deletar um registro da tabela 'cliente' com base no código
                MySqlCommand cmd = new MySqlCommand("delete from Produto where CodProd=@codigoProd", conexao);

                // Adiciona um parâmetro para o código a ser excluído, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@codigoPRod", Id);

                // Executa o comando SQL de exclusão e retorna o número de linhas afetadas
                int i = cmd.ExecuteNonQuery();

                conexao.Close(); // Fecha  a conexão com o banco de dados
            }
        }
    }
}
