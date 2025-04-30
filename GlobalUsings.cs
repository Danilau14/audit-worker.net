// System
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Diagnostics.CodeAnalysis;

global using System.Net;
global using System.Net.Mail;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;


// Microsoft

global using Microsoft.OpenApi.Models;
global using Microsoft.Extensions.Options;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;


// RabbitMQ
global using RabbitMQ.Client;
global using RabbitMQ.Client.Events;


// ParkingApi - Data, Models, DTOs, Mappings, Repositories, Services, Interfaces
global using AuditWorker.Data;
global using AuditWorker.DTOs.QueueMessage;
global using AuditWorker.Enums;
global using AuditWorker.Extensions;
global using AuditWorker.Interfaces;
global using AuditWorker.Models;
global using AuditWorker.Repositories;
global using AuditWorker.Services;
global using AuditWorker.Workers;

