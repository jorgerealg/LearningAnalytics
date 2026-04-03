# 🎓 Learning Analytics System

Una solución completa de análisis educativo con criptografía y blockchain desarrollada en C# y .NET 8.

## 📋 Descripción del Proyecto

Este proyecto académico implementa una herramienta de **Learning Analytics** que permite:

- 📊 Analizar datos educativos (notas, participación)
- 🔐 Proteger datos sensibles mediante criptografía AES-256
- ⛓️ Garantizar integridad mediante simulación de blockchain
- 🤖 Incluir predicción de rendimiento con Machine Learning
- 📈 Generar visualizaciones y reportes detallados

## 🏗️ Arquitectura del Proyecto

```
LearningAnalytics/
│
├── LearningAnalytics.Core/          # Modelos y lógica de negocio
├── LearningAnalytics.Services/      # Servicios de negocio
├── LearningAnalytics.Infrastructure/ # Acceso a datos y criptografía
├── LearningAnalytics.Console/       # Aplicación principal
└── LearningAnalytics.Tests/         # Tests unitarios
```

## 🛠️ Tecnologías Utilizadas

- **.NET 8** - Framework principal
- **C#** - Lenguaje de programación
- **AES-256** - Encriptación simétrica
- **SHA256** - Hash criptográfico
- **Blockchain simulado** - Integridad de datos
- **ML.NET** - Machine Learning
- **xUnit** - Testing unitario
- **System.Text.Json** - Serialización JSON

## 🚀 Cómo Ejecutar

### Prerrequisitos

- .NET 8 SDK instalado
- Visual Studio 2022 o VS Code

### Pasos

1. **Clonar o descargar el proyecto**

2. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

3. **Compilar la solución**
   ```bash
   dotnet build
   ```

4. **Ejecutar la aplicación**
   ```bash
   dotnet run --project LearningAnalytics.Console
   ```

5. **Ejecutar tests**
   ```bash
   dotnet test
   ```

## 📋 Funcionalidades

### 1. 🎯 Análisis Completo
- Generación de datos de ejemplo
- Encriptación de datos sensibles
- Almacenamiento en blockchain
- Análisis educativo completo
- Entrenamiento de modelo ML

### 2. 📁 Procesamiento CSV
- Lectura de archivos CSV
- Limpieza y validación de datos
- Anonimización opcional

### 3. 🔐 Criptografía
- Encriptación AES-256
- Generación de hashes SHA256
- Gestión segura de claves

### 4. ⛓️ Blockchain
- Creación de bloques
- Validación de cadena
- Exportación de datos

### 5. 🤖 Machine Learning
- Modelo de clasificación binaria
- Predicción de aprobación
- Evaluación de métricas

## 📊 Modelos de Datos

### Student
```csharp
public class Student
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Grade { get; set; }        // 0-10
    public double Participation { get; set; } // 0-10
    public DateTime CreatedAt { get; set; }
    
    public bool IsPassing => Grade >= 5.0;
    public bool IsAtRisk => Grade < 5.0 || Participation < 3.0;
}
```

### Block
```csharp
public class Block
{
    public int Index { get; set; }
    public DateTime Timestamp { get; set; }
    public string Data { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }
    public int Nonce { get; set; }
}
```

## 🔧 Justificación Técnica

### AES vs RSA
- **AES-256**: Encriptación simétrica rápida y eficiente para grandes volúmenes de datos
- **RSA**: Sería más lenta y compleja para este caso de uso
- **Decisión**: AES-256 por su eficiencia y seguridad probada

### Blockchain Simulado
- Implementación educativa para demostrar conceptos
- Validación de integridad de datos
- Inmutabilidad garantizada mediante hashing

### Privacidad y GDPR
- Encriptación de datos personales (nombres)
- Anonimización mediante hashing
- Cumplimiento con principios de minimización de datos

## 📈 Métricas de Análisis

El sistema proporciona:

- **Estadísticas Descriptivas**: Media, mediana, desviación estándar
- **Correlación**: Entre participación y notas
- **Segmentación**: Por niveles de rendimiento
- **Predicciones**: Usando ML.NET
- **Recomendaciones**: Automáticas basadas en datos

## 🧪 Testing

El proyecto incluye tests unitarios para:

- ✅ Encriptación/desencriptación
- ✅ Validación de blockchain
- ✅ Análisis educativo
- ✅ Procesamiento de datos

Ejecutar tests:
```bash
dotnet test --logger "console;verbosity=detailed"
```

## 📁 Estructura de Archivos

```
LearningAnalytics.Core/
├── Models/
│   ├── Student.cs
│   ├── EncryptedStudent.cs
│   ├── Block.cs
│   └── AnalyticsResult.cs

LearningAnalytics.Services/
├── Services/
│   ├── IBlockchainService.cs
│   ├── BlockchainService.cs
│   ├── IDataProcessingService.cs
│   ├── DataProcessingService.cs
│   ├── IAnalyticsService.cs
│   ├── AnalyticsService.cs
│   ├── IMachineLearningService.cs
│   └── MachineLearningService.cs

LearningAnalytics.Infrastructure/
├── Services/
│   ├── ICryptoService.cs
│   └── CryptoService.cs

LearningAnalytics.Console/
├── Program.cs
└── UI/
    └── DisplayManager.cs

LearningAnalytics.Tests/
├── CryptoServiceTests.cs
├── BlockchainServiceTests.cs
└── AnalyticsServiceTests.cs
```

## 🎮 Uso Interactivo

Al ejecutar la aplicación, verás un menú con las siguientes opciones:

1. **🎯 Ejecutar análisis completo** - Flujo completo del sistema
2. **📁 Cargar y analizar CSV** - Procesar archivos externos
3. **🎲 Generar datos de ejemplo** - Crear datos de prueba
4. **🔐 Probar criptografía** - Demostración de encriptación
5. **⛓️ Probar blockchain** - Validación de cadena
6. **🤖 Probar Machine Learning** - Entrenar y evaluar modelos
7. **📊 Ver información de blockchain** - Inspeccionar cadena
8. **💾 Exportar datos** - Guardar resultados
9. **🚪 Salir** - Cerrar aplicación

## 📊 Ejemplo de Salida

```
📊 RESULTADOS DEL ANÁLISIS EDUCATIVO

📈 ESTADÍSTICAS GENERALES:
   • Total estudiantes: 50
   • Promedio notas: 7.34
   • Promedio participación: 6.78
   • Tasa aprobación: 78.0%
   • Tasa riesgo: 16.0%
   • Correlación participación-nota: 0.734

💡 RECOMENDACIONES:
   • Existe una fuerte correlación entre participación y notas
   • 8 estudiantes (16.0%) están en riesgo académico
   • Considerar estrategias para incrementar la interacción

🤖 MACHINE LEARNING:
   • Estado: ✅ Entrenado exitosamente
   • Precisión: 0.856
   • Muestras: 50
```

## 🔮 Características Futuras

- [ ] Interfaz web con ASP.NET Core
- [ ] Base de datos real (SQL Server/PostgreSQL)
- [ ] Más algoritmos de ML
- [ ] Dashboard en tiempo real
- [ ] Integración con sistemas LMS
- [ ] Exportación a múltiples formatos

## 📄 Licencia

Este proyecto es para fines educativos y académicos.

## 👥 Autores

Desarrollado como proyecto académico para demostrar integración de:
- Criptografía y seguridad
- Blockchain y integridad de datos
- Machine Learning y análisis predictivo
- Principios SOLID y clean code

---

**Nota**: Este es un proyecto educativo. Para producción, considere aspectos adicionales de seguridad, escalabilidad y rendimiento.
