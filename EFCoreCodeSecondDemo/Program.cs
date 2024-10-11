// See https://aka.ms/new-console-template for more information
using EFCoreCodeSecondDemo.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

try
{
    // Initialize the DbContext
    using (var context = new EFCoreDbContext())
    {
        //Sample1
        // Define the search criteria (searching for a student with the first name "Alice")
        string searchFirstName = "Alice";

        // LINQ Query Syntax to search for a student by first name
        var searchResultQS = (from student in context.Students
                              where student.FirstName == searchFirstName
                              select student).ToList();

        // LINQ Method Syntax to search for a student by first name
        var searchResultMS = context.Students //accesses the Students DbSet
                                  .Where(s => s.FirstName == searchFirstName) //filters students with the given first name
                                  .ToList(); //executes the query and returns the result as a list

        // Check if any student is found
        if (searchResultQS.Any())
        {
            // Iterate through the result and display the student's details
            foreach (var student in searchResultQS)
            {
                Console.WriteLine($"Student Found: {student.FirstName} {student.LastName}, Email: {student.Email}");
            }
        }
        else
        {
            // Output if no student is found
            Console.WriteLine("No student found with the given first name.");
        }

        //Sample2
        // Define the filtering criteria
        string branchName = "Computer Science Engineering"; // Branch name filter
        string gender = "Female"; // Gender filter

        // LINQ Query Syntax to filter students by branch name and gender with eager loading
        var filteredStudentsQS = (from student in context.Students
                                 .Include(s => s.Branch) // Eager loading of the Branch property
                                  where student.Branch.BranchName == branchName && student.Gender == gender
                                  select student).ToList();

        // LINQ Method Syntax to filter students by branch name and gender with eager loading
        var filteredStudents = context.Students
                                      .Include(s => s.Branch) // Eager loading of the Branch property
                                      .Where(s => s.Branch.BranchName == branchName && s.Gender == gender)
                                      .ToList();

        // Check if any students match the filtering criteria
        if (filteredStudentsQS.Any())
        {
            // Iterate through the filtered students and display their details
            foreach (var student in filteredStudentsQS)
            {
                Console.WriteLine($"Student Found: {student.FirstName} {student.LastName}, Branch: {student.Branch.BranchName}, Gender: {student.Gender}");
            }
        }
        else
        {
            // Output if no students match the filtering criteria
            Console.WriteLine("No students found matching the given criteria.");
        }

        //Sample3
        // Sorting students by Gender ascending and EnrollmentDate descending using Query Syntax
        var sortedStudentsQuerySyntax = (from student in context.Students
                                         orderby student.Gender ascending, student.EnrollmentDate descending
                                         select student).ToList();
        // Sorting students by LastName ascending and EnrollmentDate descending using Method Syntax
        var sortedStudentsMethodSyntax = context.Students
                                                .OrderBy(s => s.Gender) // Primary sort by Gender in ascending order
                                                .ThenByDescending(s => s.EnrollmentDate) // Secondary sort by EnrollmentDate in descending order
                                                .ToList();
        // Check if any students are found
        if (sortedStudentsQuerySyntax.Any())
        {
            // Iterate through the sorted students and display their details
            foreach (var student in sortedStudentsQuerySyntax)
            {
                // Output the student's details including Gender and enrollment date
                Console.WriteLine($"Student: {student.LastName} {student.FirstName}, Gender: {student.Gender}, Enrollment Date: {student.EnrollmentDate.ToShortDateString()}");
            }
        }
        else
        {
            // Output if no students are found
            Console.WriteLine("No students found.");
        }

        //Sample4
        // Grouping students by their Branch using Query Syntax
        var groupedStudentsQuerySyntax = (from student in context.Students
                                         .Include(s => s.Branch) // Eager loading of the Branch property
                                          group student by student.Branch.BranchName into studentGroup //Group Students by BranchName into studentGroup
                                          select new
                                          {
                                              // studentGroup.Key is the BranchName in this case
                                              BranchName = studentGroup.Key,

                                              // Count the number of students in each group
                                              StudentCount = studentGroup.Count()
                                          }).ToList();
        // Grouping students by their Branch using Method Syntax
        var groupedStudentsMethodSyntax = context.Students
                                                 .Include(s => s.Branch) // Eager loading of the Branch property
                                                 .GroupBy(s => s.Branch.BranchName) // Group students by BranchName
                                                 .Select(g => new
                                                 {
                                                     // g.Key is the BranchName in this case
                                                     BranchName = g.Key,

                                                     // Count the number of students in each group
                                                     StudentCount = g.Count()
                                                 })
                                                 .ToList();

        // Check if any groups are found
        if (groupedStudentsQuerySyntax.Any())
        {
            // Iterate through the grouped students and display their details
            foreach (var group in groupedStudentsQuerySyntax)
            {
                // Output the Branch name and the number of students in that branch
                Console.WriteLine($"\nBranch: {group.BranchName}, Number of Students: {group.StudentCount}");
            }
        }
        else
        {
            // Output if no students are found
            Console.WriteLine("No students found.");
        }

        //Sample5
        // Grouping students by their Branch using Query Syntax
        var groupedStudentsQuerySyntax1 = (from student in context.Students
                                         .Include(s => s.Branch) // Eager loading of the Branch property
                                          group student by student.Branch.BranchName into studentGroup
                                          select new
                                          {
                                              // studentGroup.Key is the BranchName in this case
                                              BranchName = studentGroup.Key,

                                              // Count the number of students in each group
                                              StudentCount = studentGroup.Count(),

                                              // Retrieve the list of students in each group
                                              Students = studentGroup.ToList()
                                          }).ToList();

        // Grouping students by their Branch using Method Syntax
        var groupedStudentsMethodSyntax1 = context.Students
                                                 .Include(s => s.Branch) // Eager loading of the Branch property
                                                 .GroupBy(s => s.Branch.BranchName) // Group students by BranchName
                                                 .Select(g => new
                                                 {
                                                     // g.Key is the BranchName in this case
                                                     BranchName = g.Key,

                                                     // Count the number of students in each group
                                                     StudentCount = g.Count(),

                                                     // Retrieve the list of students in each group
                                                     Students = g.ToList()
                                                 })
                                                 .ToList();

        // Check if any groups are found
        if (groupedStudentsQuerySyntax1.Any())
        {
            // Iterate through the grouped students and display their details
            foreach (var group in groupedStudentsQuerySyntax1)
            {
                // Output the Branch name and the number of students in that branch
                Console.WriteLine($"\nBranch: {group.BranchName}, Number of Students: {group.StudentCount}");

                // Display details of each student in the branch
                foreach (var student in group.Students)
                {
                    Console.WriteLine($"\tStudent: {student.FirstName} {student.LastName}, Email: {student.Email}, Enrollment Date: {student.EnrollmentDate.ToShortDateString()}");
                }
            }
        }
        else
        {
            // Output if no students are found
            Console.WriteLine("No students found.");
        }

        //Sample6
        // Joining Students and Branches using Query Syntax (LINQ query)
        var studentsWithBranchesQuerySyntax = (from student in context.Students // Loop over the Students table
                                               join branch in context.Branches // Perform an inner join with the Branches table
                                               on student.Branch.BranchId equals branch.BranchId // Define the join condition based on BranchId
                                               select new // Create an anonymous object containing selected fields from both tables
                                               {
                                                   student.FirstName, // Select the student's first name
                                                   student.LastName,  // Select the student's last name
                                                   student.Email,     // Select the student's email
                                                   student.EnrollmentDate, // Select the student's enrollment date
                                                   branch.BranchName  // Select the corresponding branch name
                                               }).ToList();
        // Execute the query and convert the result to a list
        // Joining Students and Branches using Method Syntax (LINQ method chaining)
        var studentsWithBranchesMethodSyntax = context.Students // Start with the Students table
                                                      .Join(context.Branches, // Join with the Branches table
                                                            student => student.Branch.BranchId, // Define the key from the Students table for the join (BranchId)
                                                            branch => branch.BranchId, // Define the key from the Branches table for the join (BranchId)
                                                            (student, branch) => new // Create an anonymous object for each joined record
                                                            {
                                                                student.FirstName, // Select the student's first name
                                                                student.LastName,  // Select the student's last name
                                                                student.Email,     // Select the student's email
                                                                student.EnrollmentDate, // Select the student's enrollment date
                                                                branch.BranchName  // Select the corresponding branch name
                                                            })
                                                      .ToList(); 
        // Execute the query and convert the result to a list
        // Check if any results are found
        if (studentsWithBranchesQuerySyntax.Any())
        {
            // Iterate through the results and display the details
            foreach (var item in studentsWithBranchesQuerySyntax)
            {
                // Output the student's details along with the branch name
                Console.WriteLine($"Student: {item.FirstName} {item.LastName}, Email: {item.Email}, Enrollment Date: {item.EnrollmentDate.ToShortDateString()}, Branch: {item.BranchName}");
            }
        }
        else
        {
            // Output if no students are found
            Console.WriteLine("No students found.");
        }

        //Sample7
        Console.WriteLine("==============Branch Wise Report==============");
        // LINQ Query Syntax:
        // This query joins the Branches and Students tables, groups the students by branch,
        // and then prepares to calculate additional information like the number of students 
        // and the average enrollment date for each branch.
        var branchDetailsQuerySyntax = (from branch in context.Branches
                                            // Join the Branches and Students tables on BranchId
                                        join student in context.Students on branch.BranchId equals student.Branch.BranchId
                                        // Group the students by BranchId and BranchName
                                        group student by new { branch.BranchId, branch.BranchName } into branchGroup
                                        // Select the grouped data to prepare for client-side processing
                                        select new
                                        {
                                            BranchName = branchGroup.Key.BranchName, // The name of the branch
                                            Students = branchGroup.ToList() // Fetch all students in this branch
                                        })
                                        .AsEnumerable() // Switch to client-side evaluation for further processing
                                        .Select(branch => new
                                        {
                                            BranchName = branch.BranchName, // The name of the branch
                                            StudentCount = branch.Students.Count(), // Count the number of students in the branch
                                                                                    // Calculate the average enrollment date (as ticks) of the students in the branch
                                            AverageEnrollmentDate = branch.Students.Average(s => s.EnrollmentDate.Ticks),
                                            // Sort students by LastName ascending, then by FirstName ascending
                                            Students = branch.Students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList()
                                        });
        // LINQ Method Syntax:
        // This query achieves the same goal as the above LINQ Query Syntax but using method syntax.
        var branchDetailsMethodSyntax = context.Branches
                                               // Join the Branches and Students tables on BranchId
                                               .Join(context.Students,
                                                     branch => branch.BranchId,
                                                     student => student.Branch.BranchId,
                                                     (branch, student) => new { branch, student })
                                               // Group the joined data by BranchId and BranchName
                                               .GroupBy(bs => new { bs.branch.BranchId, bs.branch.BranchName })
                                               .AsEnumerable() // Switch to client-side evaluation for further processing
                                               .Select(g => new
                                               {
                                                   BranchName = g.Key.BranchName, // The name of the branch
                                                   StudentCount = g.Count(), // Count the number of students in the branch
                                                                             // Calculate the average enrollment date (as ticks) of the students in the branch
                                                   AverageEnrollmentDate = g.Average(bs => bs.student.EnrollmentDate.Ticks),
                                                   // Sort students by LastName ascending, then by FirstName ascending
                                                   Students = g.Select(bs => bs.student).OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList()
                                               })
                                               .ToList(); // Convert the result to a list for further processing
                                                          // Display the results:
                                                          // Check if there are any branches with students to display
        if (branchDetailsQuerySyntax.Any())
        {
            // Iterate over each branch in the query result
            foreach (var branch in branchDetailsQuerySyntax)
            {
                // Display the branch name
                Console.WriteLine($"\nBranch: {branch.BranchName}");
                // Display the number of students in the branch
                Console.WriteLine($"Number of Students: {branch.StudentCount}");
                // Convert the average enrollment date (in ticks) to a DateTime and display it
                Console.WriteLine($"Average Enrollment Date: {new DateTime(Convert.ToInt64(branch.AverageEnrollmentDate)).ToShortDateString()}");
                // Display details of each student in the branch
                foreach (var student in branch.Students)
                {
                    Console.WriteLine($"    Student: {student.LastName}, {student.FirstName} - Enrollment Date: {student.EnrollmentDate.ToShortDateString()}, Email: {student.Email}");
                }
            }
        }
        else
        {
            // If no branch details are found, display a message indicating so
            Console.WriteLine("No branch details found.");
        }
    }
}
catch (Exception ex)
{
    // Exception handling: log the exception message
    Console.WriteLine($"An error occurred: {ex.Message}");
}
Console.ReadKey();
        