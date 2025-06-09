test:
	dotnet test --filter FullyQualifiedName~Fire.Blocks.Tests.$(NAME) --logger "console;verbosity=detailed"

csr:
	openssl genrsa -out fireblocks.key 4096
	openssl req -new -key fireblocks.key -out fireblocks.csr