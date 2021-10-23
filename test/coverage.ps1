# For some reason, VSTest forces every run to be output to a GUID-named folder... so, we need to delete 
# all previous result folders so we don't generate HTML reports for all the previous runs too
Remove-Item "TestResults" -Recurse -Force

# Generate the Coverlet coverage statistics
dotnet test --collect:"XPlat Code Coverage"

# Generate the HTML coverage report
reportgenerator "-reports:TestResults\*\coverage.cobertura.xml" "-targetdir:TestResults\coverage"
