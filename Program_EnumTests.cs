using System.ComponentModel;

namespace EnumTests
{
    public enum DocumentosEnum
    {
        [Description("Carteira de Identidade")]
        RG = 1000,
        [Description("Cadastro de Pessoa Física")]
        CPF = 1001,
        [Description("Carteira Nacional de Habilitação")]
        CNH = 1002,
        [Description("Título de Eleitor")]
        TituloEleitor = 1003
    }

    public static class EnumDescriptionValueHelper
    {
        public static (int, string) Get<T>(this T enumValue)
           where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return (9999, $"O objeto recebido não é um Enum válido");

            int enumCode = int.MinValue;

            string enumDescription = string.Empty;

            var fieldInfo = typeof(T).GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);

                if (attrs?.Length > 0)
                {
                    enumDescription = ((DescriptionAttribute)attrs[0]).Description;
                    
                    enumCode = (int)Convert.ChangeType(enumValue, Enum.GetUnderlyingType(typeof(T)));
                }
            }

            return string.IsNullOrEmpty(enumDescription)
                  ? (9999, "Não foi possível encontrar a descrição do Enum: " + enumValue.ToString())
                  : (enumCode, enumDescription);
        }
    }

    static class FactoryTests
    {
         public static void Main()
         {
            var x = EnumDescriptionValueHelper.Get<DocumentosEnum>(DocumentosEnum.CPF);

            Console.WriteLine($"Código { x.Item1 } com a descrição: { x.Item2 }");
         }
    }
}
