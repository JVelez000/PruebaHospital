using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PruebaHospital.Utils
{
    public class EmailHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailHelper> _logger;

        public EmailHelper(IConfiguration configuration, ILogger<EmailHelper> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> SendAppointmentConfirmationAsync(
            string patientEmail, 
            string patientName,
            string doctorName, 
            string specialty,
            DateTime appointmentDate,
            string appointmentTime)
        {
            if (string.IsNullOrEmpty(patientEmail) || !patientEmail.Contains("@"))
            {
                _logger.LogWarning($"Email del paciente inválido: {patientEmail}");
                return (false, "Email del paciente no es válido");
            }
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var smtpUsername = _configuration["EmailSettings:Username"] ?? "";
                var smtpPassword = _configuration["EmailSettings:Password"] ?? "";
                
                var fromEmail = smtpUsername;
                var fromName = _configuration["EmailSettings:FromName"] ?? "Hospital San Vicente";

                _logger.LogInformation($"🔧 Intentando enviar email desde: {fromEmail} a: {patientEmail}");
                
                if (string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
                {
                    return (false, "Configuración de email incompleta - verifica Username y Password");
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress(patientName, patientEmail));
                message.Subject = "✅ Confirmación de Cita Médica - Hospital San Vicente";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <style>
                                body {{ font-family: Arial, sans-serif; color: #333; line-height: 1.6; }}
                                .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                                .content {{ background: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px; }}
                                .appointment-details {{ background: white; padding: 20px; border-radius: 8px; border-left: 4px solid #007bff; margin: 20px 0; }}
                                .footer {{ background: #343a40; color: white; padding: 20px; text-align: center; border-radius: 0 0 10px 10px; margin-top: 20px; }}
                                .alert {{ background: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 15px 0; }}
                            </style>
                        </head>
                        <body>
                            <div style='max-width: 600px; margin: 0 auto;'>
                                <div class='header'>
                                    <h1>🏥 Hospital San Vicente</h1>
                                    <h2>Confirmación de Cita Médica</h2>
                                </div>
                                
                                <div class='content'>
                                    <p>Estimado/a <strong>{patientName}</strong>,</p>
                                    
                                    <p>Su cita médica ha sido programada exitosamente:</p>
                                    
                                    <div class='appointment-details'>
                                        <h3 style='color: #007bff; margin-top: 0;'>📋 Detalles de la Cita</h3>
                                        <p><strong>👨‍⚕️ Médico:</strong> Dr. {doctorName}</p>
                                        <p><strong>🎯 Especialidad:</strong> {specialty}</p>
                                        <p><strong>📅 Fecha:</strong> {appointmentDate:dd/MM/yyyy}</p>
                                        <p><strong>⏰ Hora:</strong> {appointmentTime}</p>
                                    </div>
                                    
                                    <div class='alert'>
                                        <h4 style='margin-top: 0;'>📝 Instrucciones importantes:</h4>
                                        <ul style='margin-bottom: 0;'>
                                            <li>Llegue 15 minutos antes de su cita</li>
                                            <li>Traer documento de identidad original</li>
                                            <li>Traer exámenes médicos previos (si aplica)</li>
                                            <li>En caso de incapacidad, notificar con 24h de anticipación</li>
                                        </ul>
                                    </div>
                                    
                                    <p><strong>📍 Ubicación:</strong><br>
                                    Hospital San Vicente<br>
                                    Calle Principal #123, Ciudad</p>
                                    
                                    <p><strong>📞 Contacto:</strong><br>
                                    Teléfono: (123) 456-7890<br>
                                    Email: info@hospitalsanvicente.com</p>
                                </div>
                                
                                <div class='footer'>
                                    <p>© {DateTime.Now.Year} Hospital San Vicente. Todos los derechos reservados.</p>
                                    <p><small>Este es un mensaje automático, por favor no responda a este correo.</small></p>
                                </div>
                            </div>
                        </body>
                        </html>"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                
                _logger.LogInformation($"📡 Conectando a {smtpServer}:{smtpPort}...");
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                
                _logger.LogInformation("🔐 Autenticando...");
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                
                _logger.LogInformation($"📤 Enviando email a {patientEmail}...");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Email enviado exitosamente a {patientEmail}");
                return (true, "Correo de confirmación enviado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando email a {patientEmail}");
                return (false, $"Error al enviar el correo: {ex.Message}");
            }
        }
        
        public async Task<(bool Success, string Message)> SendAppointmentReminderAsync(
            string patientEmail,
            string patientName,
            string doctorName,
            DateTime appointmentDate,
            string appointmentTime)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var smtpUsername = _configuration["EmailSettings:Username"] ?? "";
                var smtpPassword = _configuration["EmailSettings:Password"] ?? "";
                var fromEmail = smtpUsername;
                var fromName = _configuration["EmailSettings:FromName"] ?? "Hospital San Vicente";

                if (string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
                {
                    return (false, "Configuración de email no completada");
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress(patientName, patientEmail));
                message.Subject = "🔔 Recordatorio de Cita Médica - Hospital San Vicente";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <h2>🔔 Recordatorio de Cita Médica</h2>
                        <p>Estimado/a <strong>{patientName}</strong>,</p>
                        <p>Le recordamos que tiene una cita programada para mañana:</p>
                        <div style='background: #f8f9fa; padding: 15px; border-radius: 5px; margin: 15px 0;'>
                            <p><strong>Médico:</strong> Dr. {doctorName}</p>
                            <p><strong>Fecha:</strong> {appointmentDate:dd/MM/yyyy}</p>
                            <p><strong>Hora:</strong> {appointmentTime}</p>
                        </div>
                        <p><strong>📍 Ubicación:</strong> Hospital San Vicente - Calle Principal #123</p>
                        <p>Por favor llegue 15 minutos antes.</p>
                        <br>
                        <p>Saludos,<br>Hospital San Vicente</p>"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return (true, "Recordatorio enviado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al enviar el recordatorio: {ex.Message}");
            }
        }
    }
}