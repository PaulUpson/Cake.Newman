﻿using Xunit;
using FluentAssertions;

namespace Cake.Newman.Tests
{
    public sealed class NewmanSettingsTests
    {
        public sealed class TheFlagTests
        {
            [Fact]
            public void ShouldIncludeInsecureWhenSet()
            {
                // Given
                var fixture = new NewmanFixture { Settings = { DisableStrictSSL = true } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--insecure");
            }

            [Fact]
            public void ShouldIncludeIgnoreRedirectsWhenSet()
            {
                var fixture = new NewmanFixture { Settings = { IgnoreRedirects = true } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--ignore-redirects");
            }

            [Fact]
            public void ShouldIncludeBailWhenSet()
            {
                var fixture = new NewmanFixture { Settings = { ExitOnFirstFailure = true } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--bail");
            }

            [Fact]
            public void ShouldIncludeNoInsecureFileReadWhenSet()
            {
                var fixture = new NewmanFixture { Settings = { NoInsecureFileRead = true } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--no-insecure-file-read");
            }

            [Fact]
            public void ShouldIncludeSuppessExitCodeWhenSet()
            {
                var fixture = new NewmanFixture { Settings = { SuppressExitCode = true } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--suppress-exit-code");
            }

            [Fact]
            public void ShouldIncludeDisableUnicodeWhenSet()
            {
                var fixture = new NewmanFixture { Settings = { DisableUnicode = true } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--disable-unicode");
            }

            [Fact]
            public void ShouldIncludeVerboseWhenSet()
            {
                var fixture = new NewmanFixture { Settings = { Verbose = true } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--verbose");
            }

        }

        [Theory]
        [InlineData(1000)]
        [InlineData(5000)]
        public void ShouldSetRequestDelayWhenSet(int delay)
        {
            // Given
            var fixture = new NewmanFixture { Settings = { RequestDelay = delay } };

            // When
            var result = fixture.Run();

            // Then
            result.Args().Should().Be($"--delay-request {delay}");
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(3000)]
        public void ShouldSpecifyRequestTimeoutWhenSet(int timeout)
        {
            // Given
            var fixture = new NewmanFixture { Settings = { RequestTimeout = timeout } };

            // When
            var result = fixture.Run();

            // Then
            result.Args().Should().Be($"--timeout-request {timeout}");
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(3000)]
        public void ShouldSpecifyScriptTimeoutWhenSet(int timeout)
        {
            // Given
            var fixture = new NewmanFixture { Settings = { ScriptTimeout = timeout } };

            // When
            var result = fixture.Run();

            // Then
            result.Args().Should().Be($"--timeout-script {timeout}");
        }

        [Fact]
        public void ShouldSpecifyFolderWhenSet()
        {
            // Given
            var fixture = new NewmanFixture { Settings = { Folder = "Api Service" } };

            // When
            var result = fixture.Run();

            // Then
            result.Args().Should().Be("--folder \"Api Service\"", "Cake should have quoted this argument");
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(3000)]
        public void ShouldSpecifyIterationCountWhenSet(int iterationCount)
        {
            // Given
            var fixture = new NewmanFixture { Settings = { IterationCount = iterationCount } };

            // When
            var result = fixture.Run();

            // Then
            result.Args().Should().Be($"--iteration-count {iterationCount}");
        }

        [Theory]
        [InlineData("on")]
        [InlineData("off")]
        [InlineData("auto")]
        public void ShouldSpecifyColorWhenSet(string colorValue)
        {
            // Given
            var fixture = new NewmanFixture { Settings = { Color = colorValue } };

            // When
            var result = fixture.Run();

            // Then
            result.Args().Should().Be($"--color {colorValue}");
        }

        public sealed class TheExportPaths
        {
            [Fact]
            public void ShouldSpecifyCollectionExportPath()
            {
                // Given
                var path = "./export/collection.json";
                var fixture = new NewmanFixture { Settings = { ExportCollectionPath = path } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--export-collection \"export/collection.json\"", "file paths are quoted and trimmed");
            }

            [Fact]
            public void ShouldSpecifyEnvironmentExportPath()
            {
                // Given
                var fixture = new NewmanFixture { Settings = { ExportEnvironmentPath = "./export/environment.json" } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--export-environment \"export/environment.json\"", "file paths are quoted and trimmed");
            }

            [Fact]
            public void ShouldSpecifyGlobalsPath()
            {
                // Given
                var fixture = new NewmanFixture { Settings = { ExportGlobalsPath = "./export/globals.json" } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--export-globals \"export/globals.json\"", "file paths are quoted and trimmed");
            }
        }

        public sealed class TheFilePaths
        {
            [Fact]
            public void ShouldSpecifyEnvironmentsFile()
            {
                // Given
                var fixture = new NewmanFixture { Settings = { EnvironmentFile = "./collection.json" } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--environment \"collection.json\"", "file paths are quoted and trimmed");
            }

            [Fact]
            public void ShouldSpecifyGlobalsFile()
            {
                // Given
                var fixture = new NewmanFixture { Settings = { GlobalVariablesFile = "./vars/globals.json" } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--globals \"vars/globals.json\"", "file paths are quoted and trimmed");
            }

            [Fact]
            public void ShouldSpecifyDataFile()
            {
                // Given
                var fixture = new NewmanFixture { Settings = { DataFile = "./vars/data.json" } };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("-d \"vars/data.json\"", "file paths are quoted and trimmed");
            }
        }

        public sealed class TheSSLClientCertificateSettings
        {
            [Fact]
            public void ShouldSpecifyCertFile()
            {
                // Given
                var fixture = new NewmanFixture 
                { 
                    Settings = { 
                        SSLClientCertificateSettings = new SSLClientCertificateSettings 
                        {
                            Cert = "./certs/client.cert"
                        } 
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--ssl-client-cert \"certs/client.cert\"", "file paths are quoted and trimmed");
            }

            [Fact]
            public void ShouldSpecifyKeyFile()
            {
                // Given
                var fixture = new NewmanFixture 
                { 
                    Settings = { 
                        SSLClientCertificateSettings = new SSLClientCertificateSettings 
                        {
                            Key = "./certs/client.key"
                        } 
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--ssl-client-key \"certs/client.key\"", "file paths are quoted and trimmed");
            }

            [Fact]
            public void ShouldSpecifyPassphrase()
            {
                // Given
                var fixture = new NewmanFixture 
                { 
                    Settings = { 
                        SSLClientCertificateSettings = new SSLClientCertificateSettings 
                        {
                            Passphrase = "password1234"
                        } 
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                result.Args().Should().Be("--ssl-client-passphrase \"password1234\"", "passwords are quoted and trimmed");
            }
        }
    }
}
