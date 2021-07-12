using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using System;
using System.Data;
using System.Security.Claims;
using TSHFYPWebPortal2.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;


using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

public class MailSenderController : Controller
{
    public IActionResult Contact()
    {
        return View();
    }
    public string SendEmail(string Name, string Email, string Message){
        
        try
        {
            // Credentials
            var credentials = new NetworkCredential("FeedbackTSH@gmail.com", "FYPTsH!1");

            // Mail message to clients
            var mail = new MailMessage()
            {
                From = new MailAddress("FeedbackTSH@gmail.com"),
                Subject = "DoNotReply-FeedbackTSH@gmail.com",
                Body = "Feedback Sent Successfully </br> " +
                "Thank You for your response! We will update you shortly on your feedback"
            };


            // Mail message to TSH
            var mail2Me = new MailMessage()
            {
                From = new MailAddress("FeedbackTSH@gmail.com"),
                Subject = Name + ",Client's Feedback",
                Body = Message
            };

            mail.IsBodyHtml = true;
            mail.To.Add(new MailAddress(Email));

            mail2Me.IsBodyHtml = true;
            mail2Me.To.Add(new MailAddress("FeedbackTSH@gmail.com"));

            // Smtp client
            var client = new SmtpClient()
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Credentials = credentials
            };

            client.Send(mail);
            client.Send(mail2Me);

            return "Email Sent Successfully!";
        }
        catch (System.Exception e)
        {
            return e.Message;
        }
    }
}