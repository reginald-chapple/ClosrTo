using Microsoft.EntityFrameworkCore;
using Mvc.Data;
using Mvc.Entities;

namespace Mvc.Services;

public class AssessmentService
{
    private readonly ApplicationDbContext _context;

    public AssessmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Step 1: The user starts the assessment, so we fetch all questions.
    public async Task<List<AssessmentQuestion>> GetAssessmentQuestions()
    {
        return await _context.AssessmentQuestions.Include(q => q.Vice).ToListAsync();
    }

    // Step 2: The user submits their answers. We process and save them.
    public async Task<Assessment> SubmitAssessment(string userId, Dictionary<int, int> answers)
    {
        // Create a new assessment instance for this user
        var assessment = new Assessment { UserId = userId };
        _context.Assessments.Add(assessment);

        // Create response objects for each answer
        foreach (var answer in answers)
        {
            assessment.Responses.Add(new AssessmentResponse
            {
                AssessmentQuestionId = answer.Key,
                Score = answer.Value
            });
        }

        // Calculate the results
        var results = CalculateResults(assessment);
        foreach (var result in results)
        {
            assessment.Results.Add(result);
        }

        await _context.SaveChangesAsync();
        return assessment;
    }

    // Helper method to calculate the total score for each vice
    private List<AssessmentResult> CalculateResults(Assessment assessment)
    {
        // This query groups the responses by ViceId and sums the scores.
        var results = assessment.Responses
           .GroupBy(r => r.AssessmentQuestion.ViceId)
           .Select(group => new AssessmentResult
            {
                ViceId = group.Key,
                TotalScore = group.Sum(r => r.Score),
                Assessment = assessment
            }).ToList();
            
        return results;
    }

    // Step 3: After saving, we retrieve the results to find the highest-scoring vice.
    public async Task<Virtue> GetRecommendedVirtueTrack(string assessmentId)
    {
        var topResult = await _context.AssessmentResults
           .Where(r => r.AssessmentId == assessmentId)
           .OrderByDescending(r => r.TotalScore)
           .Include(r => r.Vice)
           .ThenInclude(v => v.CorrespondingVirtue)
           .FirstOrDefaultAsync();

        return topResult?.Vice.CorrespondingVirtue;
    }
}