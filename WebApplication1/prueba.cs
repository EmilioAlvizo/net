using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.IO;

public class prueba
{
    public class ApiResponse
    {
        public bool ok { get; set; }
        public List<string> errors { get; set; }
    }

    public static async Task Correr()
    {
        var handler = new HttpClientHandler()
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };

        var client = new HttpClient(handler);

        // 🔥 HEADERS IMPORTANTES
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
        client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        client.DefaultRequestHeaders.Add("Origin", "https://pruebas.juventudesgente.gob.mx");
        client.DefaultRequestHeaders.Add("Referer", "https://pruebas.juventudesgente.gob.mx/");

        string baseUrl = "https://pruebas.juventudesgente.gob.mx";
        string rutaInicial = "https://pruebas.juventudesgente.gob.mx/credit/?r=OW1JTFk4ZVFVWDJhdEpNdkJvUnIyZz09";

        // 🔥 INICIALIZAR SESIÓN
        await client.GetAsync(rutaInicial);

        // 🔥 NUEVO (MUY IMPORTANTE)
        await client.GetAsync($"{baseUrl}/credit/Notifications/getSocketToken");

        // 🔥 VALIDAR SESIÓN
        await client.GetAsync($"{baseUrl}/credit/Auth/verifyExistSession");

        var curps = new List<string>
        {
            "HIVG560521HGTDLD08",
            "AAGG751029HGTLRN04",
            "ZARR690810HGTMVM03",
            "EOJR540123HMSSRM02"
        };

        var resultados = new List<(string curp, string estado, string respuesta)>();

        foreach (var curp in curps)
        {
            try
            {
                Console.WriteLine($"Probando {curp}");

                // Paso 1
                await client.PostAsync(
                    $"{baseUrl}/credit/Validation/globalCurpValidationRequest/{curp}",
                    null
                );

                // Paso 2
                await client.PostAsync(
                    $"{baseUrl}/credit/Validation/validateModalPreValidation",
                    null
                );

                // Paso 3 (resultado final)
                var res = await client.PostAsync(
                    $"{baseUrl}/credit/Validation/validateModalPreValidationLoadRequest/",
                    null
                );

                var body = await res.Content.ReadAsStringAsync();

                // 🔍 DEBUG
                Console.WriteLine($"STATUS: {res.StatusCode}");
                Console.WriteLine($"BODY LENGTH: {body.Length}");
                Console.WriteLine($"BODY RAW: {body}");

                string estado = "DESCONOCIDO";
                string mensaje = body;

                try
                {
                    var data = JsonSerializer.Deserialize<ApiResponse>(body);

                    if (data != null)
                    {
                        if (data.ok)
                        {
                            estado = "VÁLIDO";
                            mensaje = "OK";
                        }
                        else if (data.errors != null && data.errors.Count > 0)
                        {
                            mensaje = string.Join(" | ", data.errors);

                            if (mensaje.Contains("correo"))
                            {
                                estado = "VÁLIDO (correo duplicado)";
                            }
                            else
                            {
                                estado = "NO VÁLIDO";
                            }
                        }
                    }
                }
                catch
                {
                    estado = "RESPUESTA NO JSON";
                }

                // Limpiar HTML
                mensaje = Regex.Replace(mensaje, "<.*?>", "");

                resultados.Add((curp, estado, mensaje));

                await Task.Delay(2000);
            }
            catch (Exception ex)
            {
                resultados.Add((curp, "ERROR", ex.Message));
            }
        }

        // 📊 Crear Excel con nombre dinámico
        var fileName = $"Resultados_CURP_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        using (var workbook = new XLWorkbook())
        {
            var ws = workbook.Worksheets.Add("Resultados");

            ws.Cell(1, 1).Value = "CURP";
            ws.Cell(1, 2).Value = "Estado";
            ws.Cell(1, 3).Value = "Respuesta";

            for (int i = 0; i < resultados.Count; i++)
            {
                ws.Cell(i + 2, 1).Value = resultados[i].curp;
                ws.Cell(i + 2, 2).Value = resultados[i].estado;
                ws.Cell(i + 2, 3).Value = resultados[i].respuesta ?? "SIN RESPUESTA";
            }

            ws.Columns().AdjustToContents();

            workbook.SaveAs(fileName);
        }

        Console.WriteLine($"Excel generado: {fileName} 🚀");
    }
}